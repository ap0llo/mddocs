namespace Grynwald.MdDocs.Common.Configuration
{

    public class CommandLineHelpConfiguration
    {
        public bool IncludeVersion { get; set; }

    }


    public class ApiReferenceConfiguration
    {
    }

    public enum MarkdownPreset
    {
        Default,
        MkDocs
    }

    public class MarkdownConfiguration
    {
        public MarkdownPreset Preset { get; set; } = MarkdownPreset.Default;
    }

    public class DocsConfiguration
    {

        public CommandLineHelpConfiguration CommandLineHelp { get; set; } = new CommandLineHelpConfiguration();


        public ApiReferenceConfiguration ApiReference { get; set; } = new ApiReferenceConfiguration();

        public MarkdownConfiguration Markdown { get; set; } = new MarkdownConfiguration();
    }
}
