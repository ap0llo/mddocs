using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class CommandParametersTable : MdPartial
    {
        private readonly CommandDocumentation m_Command;
        private readonly Func<OptionDocumentation, string> m_GetOptionAnchor;
        private readonly Func<ValueDocumentation, string> m_GetValueAnchor;


        public CommandParametersTable(CommandDocumentation command, Func<OptionDocumentation, string> getOptionAnchor, Func<ValueDocumentation, string> getValueAnchor)
        {
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
            m_GetOptionAnchor = getOptionAnchor ?? throw new ArgumentNullException(nameof(getOptionAnchor));
            m_GetValueAnchor = getValueAnchor ?? throw new ArgumentNullException(nameof(getValueAnchor));
        }


        //TODO: Add Required true/false
        protected override MdBlock ConvertToBlock()
        {
            MdSpan GetLinkOrEmptySpan(string text, string uri, bool italic = false)
            {
                return String.IsNullOrEmpty(text) ? (MdSpan)MdEmptySpan.Instance : new MdLinkSpan(italic ? new MdEmphasisSpan(text) : (MdSpan)text, uri);
            }

            var addNameColumn = m_Command.Options.Any(x => !String.IsNullOrEmpty(x.Name));
            var addShortNameColumn = m_Command.Options.Any(x => x.ShortName != null);
            var addPositionColumn = m_Command.Values.Count > 0;

            var headerRow = new MdTableRow();

            headerRow.AddIf(addPositionColumn, "Position");
            headerRow.AddIf(addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && !addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && addNameColumn, "Short Name");

            headerRow.Add("Description");

            var table = new MdTable(headerRow);

            foreach (var value in m_Command.Values.OrderBy(x => x.Index))
            {
                var link = "#" + m_GetValueAnchor(value);

                var row = new MdTableRow
                {
                    value.Index.ToString()
                };
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(value.Name, link, italic: true));

                row.AddIf(addShortNameColumn && !addNameColumn, GetLinkOrEmptySpan(value.Name, link, italic: true));
                row.AddIf(addShortNameColumn && addNameColumn, MdEmptySpan.Instance);

                row.Add(value.HelpText ?? "");

                table.Add(row);
            }

            foreach (var option in m_Command.Options)
            {
                var link = "#" + m_GetOptionAnchor(option);

                var row = new MdTableRow();

                row.AddIf(addPositionColumn, MdEmptySpan.Instance);
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(option.Name, link));
                row.AddIf(addShortNameColumn, GetLinkOrEmptySpan(option.ShortName?.ToString(), link));
                row.Add(option.HelpText ?? "");

                table.Add(row);
            }

            return table;
        }
    }
}
