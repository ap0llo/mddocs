using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData
{
    [Verb("command1", HelpText = "Some Help Text")]
    public class Command1Options : OptionsBase
    {
        public override int AbstractParameter { get; set; }
    }
}
