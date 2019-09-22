using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData
{
    public abstract class OptionsBase
    {
        [Option("parameterFromBaseClass")]
        public int ParameterFromBaseClass { get; set; }

        [Option("abstractParameter")]
        public abstract int AbstractParameter { get; set; }
    }
}
