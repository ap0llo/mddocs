using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData.SingleCommandApp
{    
    public class Options
    {
        [Option("option1", Required = true)]
        public string Option1 { get; set; }

        [Option("option2", Required = true)]
        public int Option2 { get; set; }

        [Option("option3", Required = true, Hidden = true)]
        public int Option3 { get; set; }

        [Option('x', "option4", Required = true)]
        public string Option4 { get; set; }

        [Option('y', "option5", Required = true)]
        public int Option5 { get; set; }

        [Value(0)]
        public string Value1 { get; set; }

        [Value(1, MetaName = "Value2 name")]
        public string Value2 { get; set; }
    }
}
