using System;
using System.IO;
using CommandLine;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs
{
    /// <summary>
    /// Options for the "apireference" command.
    /// </summary>
    [Verb("apireference", HelpText = "Generate API reference documentation for a .NET assembly.")]
    internal class ApiReferenceOptions : OptionsBase
    {
        private string? m_AssemblyPath;

        [Option('a', "assembly", Required = false, HelpText = "Path of the assembly to generate documentation for.")]
        [ConfigurationValue("mddocs:apireference:assemblyPath")]
        public string? AssemblyPath
        {
            // If output directory has a value, convert it to a full path.
            // Otherwise, a relative path will be interpreted to be relative to the
            // configuration file path by the configuration system
            get => String.IsNullOrWhiteSpace(m_AssemblyPath) ? m_AssemblyPath : Path.GetFullPath(m_AssemblyPath);
            set => m_AssemblyPath = value;
        }

        [ConfigurationValue("mddocs:apireference:outputPath")]
        public override string? OutputDirectory
        {
            get => base.OutputDirectory;
            set => base.OutputDirectory = value;
        }
    }
}
