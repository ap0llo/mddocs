using Grynwald.MdDocs.CommandLineHelp.Commands;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:commandlinehelp:assemblyPath")]
        public string AssemblyPath => Assembly.GetFullPath();

        [ConfigurationValue("mddocs:commandlinehelp:outputPath")]
        public string OutputDirectoryPath => OutputDirectory?.GetFullPath() ?? "";

        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var configuration = GetConfigurationProvider().GetCommandLineHelpConfiguration();
            var command = new CommandLineHelpCommand(Logger, configuration);
            var success = command.Execute();
            return success && (Log.HasLoggedErrors == false);
        }
    }
}
