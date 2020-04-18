using CommandLine;
using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs
{
    internal abstract class OptionsBase
    {
        [Option('v', "verbose", HelpText = "Show more detailed log output.")]
        public bool Verbose { get; set; }

        [Option('o', "outdir",
            Required = true,
            HelpText = "Path of the directory to write the documentation to. If the output directory already exists, " +
                       "all files in the output directory will be deleted.")]
        public string? OutputDirectory { get; set; }

        [Option("markdown-preset",
            Required = false,
            Default = MarkdownPreset.Default,
            HelpText = "Specifies the preset to use for generating Markdown files.")]
        [ConfigurationValue("mddocs:markdown:preset")]
        public MarkdownPreset MarkdownPreset { get; set; }

        [Option('c', "configurationFilePath", Required = false, HelpText =
            "The path of the configuration file to use. " +
            "If no configuration file is specified, default settings are used.")]
        public string? ConfigurationFilePath { get; set; }

    }
}
