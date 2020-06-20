using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Templates.Default;
using Grynwald.MdDocs.Common.Templates;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Templates
{
    public static class CommandLineHelpTemplateProvider
    {
        public static ITemplate<ApplicationDocumentation> GetTemplate(ILogger logger, CommandLineHelpConfiguration configuration)
        {
            switch (configuration.Template.Name)
            {
                case CommandLineHelpConfiguration.TemplateName.Default:
                    return new CommandLineHelpDefaultTemplate(configuration, new DefaultCommandLineHelpPathProvider(), logger);

                default:
                    throw new InvalidTemplateConfigurationException($"Unknown template '{configuration.Template.Name}'");
            }
        }

    }
}
