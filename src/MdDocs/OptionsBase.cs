using System;
using System.IO;
using CommandLine;
using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs
{
    internal abstract class OptionsBase
    {
        private string? m_OutputDirectory;


        [Option('v', "verbose", HelpText = "Show more detailed log output.")]
        public bool Verbose { get; set; }

        [Option('o', "outdir",
            HelpText = "Path of the directory to write the documentation to. If the output directory already exists, " +
                       "all files in the output directory will be deleted.")]
        public virtual string? OutputDirectory
        {
            // If output directory has a value, convert it to a full path.
            // Otherwise, a relative path will be interpreted to be relative to the
            // configuration file path by the configuration system
            get => String.IsNullOrWhiteSpace(m_OutputDirectory) ? m_OutputDirectory : Path.GetFullPath(m_OutputDirectory);
            set => m_OutputDirectory = value;
        }

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
