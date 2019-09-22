using CommandLine;

namespace Grynwald.MdDocs
{
    /// <summary>
    /// Options for the "commandlinehelp" command.
    /// </summary>
    [Verb("commandlinehelp", HelpText = "Generate command line help for .NET console application implemented using the 'CommandLineParser' package")]
    internal class CommandLineHelpOptions : OptionsBase
    {
        [Option('a', "assembly", Required = true, HelpText = "Path of the command line application assembly to generate documentation for.")]
        public string AssemblyPath { get; set; }

        [Option('o', "outdir", Required = true, HelpText = "Path of the directory to write the documentation to.")]
        public string OutputDirectory { get; set; }
    }
}
