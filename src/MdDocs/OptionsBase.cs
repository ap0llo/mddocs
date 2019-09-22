using CommandLine;

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
        public string OutputDirectory { get; set; }
    }
}
