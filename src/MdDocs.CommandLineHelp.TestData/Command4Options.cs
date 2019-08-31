using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData
{
    [Verb("command4")]
    public class Command4Options
    {
        [Value(0)]
        public string Value1 { get; set; }

        [Value(1, MetaName = "Value2 name")]
        public string Value2 { get; set; }

        [Value(2, HelpText = "Value 3 Help text")]
        public string Value3 { get; set; }

        [Value(3, Hidden = true)]
        public string Value4 { get; set; }

        [Value(4, Default = "Value 5 Default")]
        public string Value5 { get; set; }
    }
}
