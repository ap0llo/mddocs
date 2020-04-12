using System;
using CommandLine;
using CommandLine.Text;

[assembly: AssemblyUsage(
    "An application to demonstrate how the generated command lien help looks like.",
    "General usage information for the application can be specified by applying a [AssemblyUsage] attribute to the assembly.",
    "This application is a \"multi-command\" application that provides multiple subcommands.",
    "The application main page (this page) includes the application version, the usage information and a list of commands")]

namespace MdDocs.CommandLineHelp.DemoProject
{

    enum MyEnum
    {
        AcceptedValue1,
        AcceptedValue2,
        AnotherValue
    }

    [Verb("Command1",
        HelpText = "Documentation for the 'command1' subcommand. For every subcommand, a separate page will be generated.\r\n" +
        "The command page includes the commands description (provided as 'HelpText' on the [Verb] attribute.)")]
    class Command1Options
    {
        [Option("parameter1", HelpText = "The description of the named parameter1 (Declared using the [Option] attribute).")]
        public string Option1 { get; set; }

        [Option('x', "parameter2", HelpText = "For named parameters \"short\" (a single character like 'x') and \"long\" names are supported.")]
        public string Option2 { get; set; }

        [Option('y', HelpText = "Parameters without a long name are supported as well.")]
        public string Option3 { get; set; }

        [Option("parameter4", HelpText = "This is an example of a mandatory parameter", Required = true)]
        public string Option4 { get; set; }

        [Option("parameter5", HelpText = "Is a parameter is an enum, the list of accepted values will be included in the documentation.")]
        public MyEnum Option5 { get; set; }

        [Value(0, HelpText = "Unnamed parameters (declared using the [Value] attribute) are identified by index instead of name.")]
        public string Value1 { get; set; }

        [Value(1, HelpText = "If a parameter has a default value, it will be included in the documentation, too", Default = "MyDefaultValue")]
        public string Value2 { get; set; }

    }

    [Verb("Command2", HelpText = "Documentation for the 'command2' subcommand. This command has no parameters.")]
    class Command2Options
    {

    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
