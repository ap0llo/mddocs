using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class OptionDetailsSection : ParameterDetailsSection
    {
        private readonly OptionDocumentation m_Option;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Option;


        public OptionDetailsSection(OptionDocumentation option)
        {
            m_Option = option ?? throw new ArgumentNullException(nameof(option));

            var name = option.Name ?? option.ShortName.ToString();
            Heading = new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name), " Parameter"));
        }



        protected override MdBlock ConvertToBlock()
        {
            return new MdContainerBlock()
                .AddIf(true, Heading)
                .AddIf(!String.IsNullOrEmpty(m_Option.HelpText), () => new MdParagraph(m_Option.HelpText))
                .AddIf(m_Option.Default != null, () => GetDefaultValueParagraph());
        }

        private MdParagraph GetDefaultValueParagraph()
        {
            return new MdParagraph(
                new MdStrongEmphasisSpan("Default value:"),
                " ",
                new MdCodeSpan(Convert.ToString(m_Option.Default))
            );
        }
    }
}
