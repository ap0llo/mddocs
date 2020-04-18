namespace Grynwald.MdDocs.Common.Configuration
{
    public class DocsConfiguration
    {
        public class CommandLineHelpConfiguration
        {
            public bool IncludeVersion { get; set; }

        }

        public CommandLineHelpConfiguration CommandLineHelp { get; set; } = new CommandLineHelpConfiguration();
    }
}
