using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    /// <summary>
    /// Partial to render the details for a value (positional parameter)
    /// </summary>
    internal class PositionalParameterDetailsSection : ParameterDetailsSection
    {
        private readonly PositionalParameterDocumentation m_Model;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Model;


        public PositionalParameterDetailsSection(PositionalParameterDocumentation model)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));

            if (!String.IsNullOrWhiteSpace(m_Model.InformationalName))
            {
                Heading = new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(m_Model.InformationalName), $" Parameter (Position {m_Model.Position})"));
            }
            else
            {
                //TODO: Find a better heading
                Heading = new MdHeading(3, new MdCompositeSpan($"Parameter (Position {m_Model.Position})"));
            }
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

            if (!String.IsNullOrWhiteSpace(m_Model.InformationalName))
            {
                table.Add(new MdTableRow("Informational Name:", m_Model.InformationalName));
            }

            table.Add(new MdTableRow("Position:", m_Model.Position.ToString()));

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
                table.Add(new MdTableRow("Default value:", m_Model.DefaultValue));
            }
            else
            {
                table.Add(new MdTableRow("Default value:", new MdEmphasisSpan("None")));
            }
            return table;
        }
    }
}
