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

            // Only include the "Name" column, if any option has a 'long' name
            var addNameColumn = m_Command.Options.Any(x => !String.IsNullOrEmpty(x.Name));
            // Only include the "Short Name" column if option has a short name
            var addShortNameColumn = m_Command.Options.Any(x => x.ShortName != null);
            // Only include the position column if the command has any unnamed parameters
            var addPositionColumn = m_Command.Values.Count > 0;

            // create empty table with header row
            var headerRow = new MdTableRow();
            headerRow.AddIf(addPositionColumn, "Position");
            headerRow.AddIf(addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && !addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && addNameColumn, "Short Name");
            headerRow.Add("Description");

            var table = new MdTable(headerRow);

            // Add a row for every value (unnamed parameter)
            foreach (var value in m_Command.Values.OrderBy(x => x.Index))
            {
                var link = "#" + m_GetValueAnchor(value);

                var row = new MdTableRow();

                // Add index and link to details anchor
                row.Add(GetLinkOrEmptySpan(value.Index.ToString(), link));

                // Add name (if a name of the value was specified) and link to details anchor
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(value.Name, link, italic: true));

                // If "Name" column was skipped, add the name in the "Short Name" column and link to details anchor
                // If the name was already added to the name column, add an empty cell
                row.AddIf(addShortNameColumn && !addNameColumn, GetLinkOrEmptySpan(value.Name, link, italic: true));
                row.AddIf(addShortNameColumn && addNameColumn, MdEmptySpan.Instance);

                // Add help text to "Description" column
                row.Add(value.HelpText ?? "");

                table.Add(row);
            }

            // Add a row for every option (named parameter)
            foreach (var option in m_Command.Options)
            {
                var link = "#" + m_GetOptionAnchor(option);

                var row = new MdTableRow();

                // add empty cell for position because option has no position
                row.AddIf(addPositionColumn, MdEmptySpan.Instance);

                // if specified, add name and link to details anchor
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(option.Name, link));

                // if specified, add shot name and link to details anchor
                row.AddIf(addShortNameColumn, GetLinkOrEmptySpan(option.ShortName?.ToString(), link));

                // Add help text to "Description" column
                row.Add(option.HelpText ?? "");

                table.Add(row);
            }

            return table;
        }
    }
}
