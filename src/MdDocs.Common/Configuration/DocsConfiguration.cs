namespace Grynwald.MdDocs.Common.Configuration
{

    public interface IConfigurationWithMarkdownPresetSetting
    {
        MarkdownPreset MarkdownPreset { get; }
    }

    public class CommandLineHelpConfiguration : IConfigurationWithMarkdownPresetSetting
    {
        public string OutputPath { get; set; } = "";

        public bool IncludeVersion { get; set; }

        public string AssemblyPath { get; set; } = "";

        public MarkdownPreset MarkdownPreset { get; set; } = MarkdownPreset.Default;

    }

    public class ApiReferenceConfiguration : IConfigurationWithMarkdownPresetSetting
    {
        public string OutputPath { get; set; } = "";

        public string AssemblyPath { get; set; } = "";

        public MarkdownPreset MarkdownPreset { get; set; } = MarkdownPreset.Default;
    }

    public enum MarkdownPreset
    {
        Default,
        MkDocs
    }

    public class DocsConfiguration
    {

        public CommandLineHelpConfiguration CommandLineHelp { get; set; } = new CommandLineHelpConfiguration();

        public ApiReferenceConfiguration ApiReference { get; set; } = new ApiReferenceConfiguration();
    }
}
