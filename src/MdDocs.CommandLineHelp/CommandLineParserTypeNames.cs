#pragma warning disable IDE1006 // Naming Styles: Ignore hint that const strings should be prefixed with 's_' as all these fields are public
namespace Grynwald.MdDocs.CommandLineHelp
{
    /// <summary>
    /// Defines constants for names of type names defined by the "CommandLineParser" library
    /// </summary>
    //TODO: Move to Loaders namespace
    internal static class CommandLineParserTypeNames
    {
        public const string VerbAttributeFullName = "CommandLine.VerbAttribute";
        public const string OptionAttributeFullName = "CommandLine.OptionAttribute";
        public const string ValueAttributeFullName = "CommandLine.ValueAttribute";
        public const string AssemblyUsageAttributeFullName = "CommandLine.Text.AssemblyUsageAttribute";
    }
}
#pragma warning restore IDE1006 // Naming Styles
