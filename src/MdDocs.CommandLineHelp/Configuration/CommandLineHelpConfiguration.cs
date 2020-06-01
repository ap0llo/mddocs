using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs.CommandLineHelp.Configuration
{
    public class CommandLineHelpConfiguration : IConfigurationWithMarkdownPresetSetting
    {
        [ConvertToFullPath]
        public string OutputPath { get; set; } = "";

        public bool IncludeVersion { get; set; }

        [ConvertToFullPath]
        public string AssemblyPath { get; set; } = "";

        public MarkdownPreset MarkdownPreset { get; set; } = MarkdownPreset.Default;
    }
}
