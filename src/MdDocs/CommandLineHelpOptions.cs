using System;
using System.IO;
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
        private string? m_AssemblyPath;

        [Option('a', "assembly", Required = false, HelpText = "Path of the command line application assembly to generate documentation for.")]
        [ConfigurationValue("mddocs:commandlinehelp:assemblyPath")]
        public string? AssemblyPath
        {
            // If output directory has a value, convert it to a full path.
            // Otherwise, a relative path will be interpreted to be relative to the
            // configuration file path by the configuration system
            get => String.IsNullOrWhiteSpace(m_AssemblyPath) ? m_AssemblyPath : Path.GetFullPath(m_AssemblyPath);
            set => m_AssemblyPath = value;
        }

        [Option("no-version", Required = false, HelpText = "Do not include the application version in the generated documentation")]
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
