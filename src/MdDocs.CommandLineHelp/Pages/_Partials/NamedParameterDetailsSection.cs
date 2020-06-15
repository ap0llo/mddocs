using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model2;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// Partial to render the details for a named parameter
    /// </summary>
    internal class NamedParameterDetailsSection : ParameterDetailsSection
    {
        private readonly NamedParameterDocumentation m_Model;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Model;


        public NamedParameterDetailsSection(NamedParameterDocumentation model)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));

            var name = model.Name ?? model.ShortName;
            Heading = new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name!), " Parameter"));
        }



        protected override MdBlock ConvertToBlock()
        {
            return new MdContainerBlock()
                .AddIf(true, Heading)
                .AddIf(!String.IsNullOrEmpty(m_Model.Description), () => new MdParagraph(m_Model.Description))
                .AddIf(true, GetDetailsTable());
        }


        private MdTable GetDetailsTable()
        {
            // TODO: Type

            var table = new MdTable(new MdTableRow("", ""));

            if (m_Model.HasName)
            {
                table.Add(new MdTableRow("Name:", m_Model.Name));
            }

            if (m_Model.HasShortName)
            {
                if (!m_Model.HasName)
                    table.Add(new MdTableRow("Name:", m_Model.ShortName));
                else
                    table.Add(new MdTableRow("Short name:", m_Model.ShortName));
            }

            table.Add(new MdTableRow("Position:", new MdEmphasisSpan("Named parameter")));

            table.Add(new MdTableRow("Required:", m_Model.Required ? "Yes" : "No"));

            if (!String.IsNullOrWhiteSpace(m_Model.ValuePlaceHolderName))
            {
                table.Add(new MdTableRow("Value:", m_Model.ValuePlaceHolderName));
            }

            if (m_Model.AcceptedValues != null)
            {
                table.Add(new MdTableRow("Accepted values:", m_Model.AcceptedValues.Select(x => new MdCodeSpan(x)).Join(", ")));
            }

            if (m_Model.DefaultValue != null)
            {
                table.Add(new MdTableRow("Default value:", new MdCodeSpan(m_Model.DefaultValue)));
            }
            else
            {
                table.Add(new MdTableRow("Default value:", new MdEmphasisSpan("None")));
            }
            return table;
        }
    }
}
