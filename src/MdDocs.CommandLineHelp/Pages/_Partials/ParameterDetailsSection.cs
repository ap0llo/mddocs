using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal abstract class ParameterDetailsSection : MdPartial
    {
        public abstract MdHeading Heading { get; }

        public abstract ParameterDocumentation Parameter { get; }
    }
}
