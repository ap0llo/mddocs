using CommandLine;

namespace Grynwald.MdDocs
{
    internal abstract class OptionsBase
    {
        [Option('v', "verbose", HelpText = "Show more detailed log output.")]
        public bool Verbose { get; set; }
    }
}
