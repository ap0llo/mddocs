using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    internal abstract class ParameterDetailsSection : MdPartial
    {
        public abstract MdHeading Heading { get; }

        public abstract ParameterDocumentation Parameter { get; }
    }
}
