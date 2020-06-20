using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    public class DefaultCommandLineHelpPathProvider : ICommandLineHelpPathProvider
    {
        public string GetPath(ApplicationDocumentation model) => "index.md";

        public string GetPath(CommandDocumentation model) => $"commands/{model.Name}.md";
    }
}
