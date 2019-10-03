using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData
{
    public enum SomeEnum
    {
        Value1,
        Value2,
        SomeOtherValue
    }

    [Verb("command3")]
    public class Command3Options
    {
        [Option("option1")]
        public string Option1Property { get; set; }

        [Option('x')]
        public string Option2Property { get; set; }

        [Option('y', "option3", Required = true)]
        public string Option3Property { get; set; }

        [Option("option4", HelpText = "Option 4 Help text", Hidden = true, Default = "DefaultValue")]
        public string Option4Property { get; set; }

        [Option("option5")]
        public SomeEnum Option5Property { get; set; }

        [Option("option6", Default = SomeEnum.SomeOtherValue)]
        public SomeEnum Option6Property { get; set; }
    }
}
