using CommandLine;
using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs
{
    /// <summary>
    /// Options for the "commandlinehelp" command.
    /// </summary>
    [Verb("commandlinehelp", HelpText = "Generate command line help for .NET console application implemented using the 'CommandLineParser' package")]
    internal class CommandLineHelpOptions : OptionsBase
    {
        [Option('a', "assembly",
            Required = true,
            HelpText = "Path of the command line application assembly to generate documentation for.")]
        public string? AssemblyPath { get; set; }

        [Option("no-version",
            Required = false,
            HelpText = "Do not include the application version in the generated documentation")]
        public bool NoVersion { get; set; }

        [ConfigurationValue("mddocs:commandlinehelp:includeVersion")]
        public bool IncludeVersion => !NoVersion;
    }
}
