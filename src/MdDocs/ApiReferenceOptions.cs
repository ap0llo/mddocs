using CommandLine;

namespace Grynwald.MdDocs
{
    [Verb("apireference", HelpText = "Generate API reference documentation for a .NET assembly.")]
    internal class ApiReferenceOptions
    {
        [Option('a', "assembly", Required = true, HelpText = "Path of the assembly to generate documentation for.")]
        public string AssemblyPath { get; set; }

        [Option('o', "outdir", Required = true, HelpText = "Path of the directory to write the documentation to.")]
        public string OutputDirectory { get; set; }
    }
}
