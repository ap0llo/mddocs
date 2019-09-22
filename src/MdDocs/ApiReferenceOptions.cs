using CommandLine;

namespace Grynwald.MdDocs
{
    /// <summary>
    /// Options for the "apireference" command.
    /// </summary>
    [Verb("apireference", HelpText = "Generate API reference documentation for a .NET assembly.")]
    internal class ApiReferenceOptions : OptionsBase
    {
        [Option('a', "assembly", Required = true, HelpText = "Path of the assembly to generate documentation for.")]
        public string AssemblyPath { get; set; }
    }
}
