using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model2;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal abstract class ParameterDetailsSection : MdPartial
    {
        public abstract MdHeading Heading { get; }

        public abstract ParameterDocumentation Parameter { get; }
    }
}
