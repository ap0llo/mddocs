using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    /// <summary>
    /// A partial to render a table of a command's parameters.
    /// </summary>
    internal class CommandParametersTable : MdPartial
    {
        private readonly IParameterCollection m_Model;
        private readonly Func<NamedValuedParameterDocumentation, string?> m_GetNamedParameterAnchor;
        private readonly Func<SwitchParameterDocumentation, string?> m_GetSwitchParameterAnchor;
        private readonly Func<PositionalParameterDocumentation, string?> m_GetPositionalParameterAnchor;


        public CommandParametersTable(
            IParameterCollection model,
            Func<NamedValuedParameterDocumentation, string?> getNamedParameterAnchor,
            Func<SwitchParameterDocumentation, string?> getSwitchParameterAnchor,
            Func<PositionalParameterDocumentation, string?> getPositionalParameterAnchor)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            m_GetNamedParameterAnchor = getNamedParameterAnchor ?? throw new ArgumentNullException(nameof(getNamedParameterAnchor));
            m_GetSwitchParameterAnchor = getSwitchParameterAnchor ?? throw new ArgumentNullException(nameof(getSwitchParameterAnchor));
            m_GetPositionalParameterAnchor = getPositionalParameterAnchor ?? throw new ArgumentNullException(nameof(getPositionalParameterAnchor));
        }


        protected override MdBlock ConvertToBlock()
        {
            MdSpan GetLinkOrEmptySpan(string? text, string uri, bool italic = false)
            {
                return (text == null || String.IsNullOrEmpty(text))
                    ? (MdSpan)MdEmptySpan.Instance
                    : new MdLinkSpan(italic ? new MdEmphasisSpan(text) : (MdSpan)text, uri);
            }

            // only include the "Name" column, if any option has a 'long' name
            var addNameColumn = m_Model.NamedParameters.Any(x => x.HasName) || m_Model.SwitchParameters.Any(x => x.HasName);
            // only include the "Short Name" column if option has a short name
            var addShortNameColumn = m_Model.NamedParameters.Any(x => x.HasShortName) || m_Model.SwitchParameters.Any(x => x.HasShortName);
            // only include the position column if the command has any unnamed parameters
            var addPositionColumn = m_Model.PositionalParameters.Any();
            // only add "Required" column if at least on option or value is mandatory
            var addRequiredColumn = m_Model.NamedParameters.Any(x => x.Required) || m_Model.PositionalParameters.Any(x => x.Required);

            // create empty table with header row
            var headerRow = new MdTableRow();
            headerRow.AddIf(addPositionColumn, "Position");
            headerRow.AddIf(addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && !addNameColumn, "Name");
            headerRow.AddIf(addShortNameColumn && addNameColumn, "Short Name");
            headerRow.AddIf(addRequiredColumn, "Required");
            headerRow.Add("Description");

            var table = new MdTable(headerRow);

            // add a row for every positional parameter
            foreach (var positionalParameter in m_Model.PositionalParameters.OrderBy(x => x.Position))
            {
                var link = "#" + m_GetPositionalParameterAnchor(positionalParameter);

                var row = new MdTableRow();

                // add index and link to details anchor
                row.Add(GetLinkOrEmptySpan(positionalParameter.Position.ToString(), link));

                // add name (if a name of the value was specified) and link to details anchor
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(positionalParameter.InformationalName, link, italic: true));

                // if "Name" column was skipped, add the name in the "Short Name" column and link to details anchor,
                // if the name was already added to the name column, add an empty cell
                row.AddIf(addShortNameColumn && !addNameColumn, GetLinkOrEmptySpan(positionalParameter.InformationalName, link, italic: true));
                row.AddIf(addShortNameColumn && addNameColumn, MdEmptySpan.Instance);

                // add value for "Required" column
                row.AddIf(addRequiredColumn, positionalParameter.Required ? "Yes" : "No");

                // add help text to "Description" column
                row.Add(positionalParameter.Description ?? "");

                table.Add(row);
            }

            // Add a row for every named parameter
            foreach (var namedParameter in m_Model.NamedParameters)
            {
                var link = "#" + m_GetNamedParameterAnchor(namedParameter);

                var row = new MdTableRow();

                // add empty cell for position because option has no position
                row.AddIf(addPositionColumn, MdEmptySpan.Instance);

                // if specified, add name and link to details anchor
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(namedParameter.Name, link));

                // if specified, add shot name and link to details anchor
                row.AddIf(addShortNameColumn, GetLinkOrEmptySpan(namedParameter.ShortName?.ToString(), link));

                // add value for "Required" column
                row.AddIf(addRequiredColumn, namedParameter.Required ? "Yes" : "No");

                // add help text to "Description" column
                row.Add(namedParameter.Description ?? "");

                table.Add(row);
            }

            // Add a row for every switch parameter
            foreach (var switchParameter in m_Model.SwitchParameters)
            {
                var link = "#" + m_GetSwitchParameterAnchor(switchParameter);

                var row = new MdTableRow();

                // add empty cell for position because option has no position
                row.AddIf(addPositionColumn, MdEmptySpan.Instance);

                // if specified, add name and link to details anchor
                row.AddIf(addNameColumn, GetLinkOrEmptySpan(switchParameter.Name, link));

                // if specified, add shot name and link to details anchor
                row.AddIf(addShortNameColumn, GetLinkOrEmptySpan(switchParameter.ShortName?.ToString(), link));

                // add value for "Required" column (switch parameters are never required)
                row.AddIf(addRequiredColumn, "No");

                // add help text to "Description" column
                row.Add(switchParameter.Description ?? "");

                table.Add(row);
            }



            return table;
        }
    }
}
