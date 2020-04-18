namespace Grynwald.MdDocs.Common.Configuration
{

    public class CommandLineHelpConfiguration
    {
        public bool IncludeVersion { get; set; }

    }

    public class DocsConfiguration
    {

        public CommandLineHelpConfiguration CommandLineHelp { get; set; } = new CommandLineHelpConfiguration();
    }
}
