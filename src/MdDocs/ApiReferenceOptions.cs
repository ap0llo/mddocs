using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private IEnumerable<string>? m_AssemblyPaths;

        [Option('a', "assemblies", Required = false, HelpText = "Path of the assemblies to generate documentation for (at least one assembly must be specified).", Min = 1)]
        [ConfigurationValue("mddocs:apireference:assemblyPaths")]
        public IEnumerable<string>? AssemblyPaths
        {
            // If assembly paths have a value, convert it to a full path.
            // Otherwise, a relative path will be interpreted to be relative to the
            // configuration file path by the configuration system
            get
            {
                if (m_AssemblyPaths is null)
                    return m_AssemblyPaths;

                return m_AssemblyPaths.Select(Path.GetFullPath);
            }
            set => m_AssemblyPaths = value;
        }

        [ConfigurationValue("mddocs:apireference:outputPath")]
        public override string? OutputDirectory
        {
            get => base.OutputDirectory;
            set => base.OutputDirectory = value;
        }
    }
}
