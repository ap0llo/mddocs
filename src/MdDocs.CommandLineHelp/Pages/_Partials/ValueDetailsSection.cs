using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class ValueDetailsSection : ParameterDetailsSection
    {
        private readonly ValueDocumentation m_Value;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Value;


        public ValueDetailsSection(ValueDocumentation value)
        {
            m_Value = value ?? throw new ArgumentNullException(nameof(value));


            if (!String.IsNullOrEmpty(m_Value.Name))
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
              .AddIf(m_Value.Default != null, () => GetDefaultValueParagraph());
        }

        private MdParagraph GetDefaultValueParagraph()
        {
            return new MdParagraph(
                new MdStrongEmphasisSpan("Default value:"),
                " ",
                new MdCodeSpan(Convert.ToString(m_Value.Default))
            );
        }
    }
}
