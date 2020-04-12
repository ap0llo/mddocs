using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// Partial to render the details for a value (positional parameter)
    /// </summary>
    internal class ValueDetailsSection : ParameterDetailsSection
    {
        private readonly ValueDocumentation m_Value;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Value;


        public ValueDetailsSection(ValueDocumentation value)
        {
            m_Value = value ?? throw new ArgumentNullException(nameof(value));


            if (m_Value.Name != null && !String.IsNullOrEmpty(m_Value.Name))
            {
                Heading = new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(m_Value.Name), $" Parameter (Position {m_Value.Index})"));
            }
            else
            {
                //TODO: Find a better heading
                Heading = new MdHeading(3, new MdCompositeSpan($"Parameter (Position {m_Value.Index})"));
            }
        }


        protected override MdBlock ConvertToBlock()
        {
            return new MdContainerBlock()
              .AddIf(true, Heading)
              .AddIf(!String.IsNullOrEmpty(m_Value.HelpText), () => new MdParagraph(m_Value.HelpText))
              .AddIf(true, GetDetailsTable());
        }


        private MdTable GetDetailsTable()
        {
            // TODO: Type

            var table = new MdTable(new MdTableRow("", ""));

            if (m_Value.HasName)
            {
                table.Add(new MdTableRow(new MdEmphasisSpan("Name:"), m_Value.Name));
            }

            table.Add(new MdTableRow("Position:", m_Value.Index.ToString()));

            table.Add(new MdTableRow("Required:", m_Value.Required ? "Yes" : "No"));

            if (m_Value.HasMetaValue)
            {
                table.Add(new MdTableRow("Value:", m_Value.MetaValue));
            }

            if (m_Value.HasAcceptedValues)
            {
                table.Add(new MdTableRow("Accepted values:", m_Value.AcceptedValues.Select(x => new MdCodeSpan(x)).Join(", ")));
            }

            if (m_Value.HasDefault)
            {
                table.Add(new MdTableRow("Default value:", Convert.ToString(m_Value.Default)));
            }
            else
            {
                table.Add(new MdTableRow("Default value:", new MdEmphasisSpan("None")));
            }
            return table;
        }
    }
}
