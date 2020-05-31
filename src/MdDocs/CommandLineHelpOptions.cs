using CommandLine;
using Grynwald.Utilities.Configuration;

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
        public bool? IncludeVersion => NoVersion ? (bool?)true : null;

        [ConfigurationValue("mddocs:commandlinehelp:outputPath")]
        public override string? OutputDirectory
        {
            get => base.OutputDirectory;
            set => base.OutputDirectory = value;
        }
    }
}
