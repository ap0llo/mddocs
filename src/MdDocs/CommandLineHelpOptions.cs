using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Grynwald.MdDocs
{
    [Verb("commandlinehelp", HelpText = "TODO")]
    internal class CommandLineHelpOptions
    {
        [Option('a', "assembly", Required = true, HelpText = "Path of the commandline applicatoin assembly to generate documentation for.")]
        public string AssemblyPath { get; set; }

        [Option('o', "outdir", Required = true, HelpText = "Path of the directory to write the documentation to.")]
        public string OutputDirectory { get; set; }

        [Option('v', "verbose", HelpText = "Show more detailed log output.")]
        public bool Verbose { get; set; }
    }
}
