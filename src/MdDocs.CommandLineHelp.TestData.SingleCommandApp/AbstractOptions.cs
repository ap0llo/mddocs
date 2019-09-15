using CommandLine;

namespace Grynwald.MdDocs.CommandLineHelp.TestData.SingleCommandApp
{
    // abstract option classes must be ignored when loading option classes
    public abstract class AbstractOptions
    {
        [Option("option1", Required = true)]
        public string Option1 { get; set; }       
    }
}
