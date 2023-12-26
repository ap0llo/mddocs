using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    public class DefaultCommandLineHelpPathProvider : ICommandLineHelpPathProvider
    {
        private readonly string m_ApplicationDocumentationName;

        public DefaultCommandLineHelpPathProvider(string applicationDocumentationName = CommandLineHelpConfiguration.DefaultTemplateConfiguration.s_DefaultApplicationMdFileName)
        {
            m_ApplicationDocumentationName = applicationDocumentationName;
        }

        public string GetPath(ApplicationDocumentation model) => m_ApplicationDocumentationName;

        public string GetPath(CommandDocumentation model) => $"commands/{model.Name}.md";
    }
}
