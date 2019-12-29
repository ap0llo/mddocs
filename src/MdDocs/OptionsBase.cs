#nullable disable

using CommandLine;

namespace Grynwald.MdDocs
{
    internal enum MarkdownPresetName
    {
        Default,
        MkDocs
    }

    internal abstract class OptionsBase
    {
        [Option('v', "verbose", HelpText = "Show more detailed log output.")]
        public bool Verbose { get; set; }

        [Option('o', "outdir",
            Required = true,
            HelpText = "Path of the directory to write the documentation to. If the output directory already exists, " +
                       "all files in the output directory will be deleted.")]
        public string OutputDirectory { get; set; }

        [Option("markdown-preset",
            Required = false,
            Default = MarkdownPresetName.Default,
            HelpText = "Specifies the preset to use for generating Markdown files.")]
        public MarkdownPresetName MarkdownPreset { get; set; }
    }
}
