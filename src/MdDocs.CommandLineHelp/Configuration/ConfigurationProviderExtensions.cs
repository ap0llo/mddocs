using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs.CommandLineHelp.Configuration
{
    public static class ConfigurationProviderExtensions
    {
        public static CommandLineHelpConfiguration GetCommandLineHelpConfiguration(this ConfigurationProvider configurationLoader)
        {
            return configurationLoader.GetConfiguration<CommandLineHelpConfiguration>("commandlinehelp");
        }

        public static CommandLineHelpConfiguration GetDefaultCommandLineHelpConfiguration(this ConfigurationProvider configurationLoader)
        {
            return configurationLoader.GetDefaultConfiguration<CommandLineHelpConfiguration>("commandlinehelp");
        }
    }
}
