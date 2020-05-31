using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:commandlinehelp:assemblyPath")]
        public string AssemblyPath => Assembly.GetFullPath();

        [ConfigurationValue("mddocs:commandlinehelp:outputPath")]
        public string OutputDirectoryPath => OutputDirectory?.GetFullPath() ?? "";

        [ConfigurationValue("mddocs:commandlinehelp:includeVersion")]
        public bool IncludeVersion { get; set; } = true;


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var configuration = LoadConfiguration();

            using (var model = ApplicationDocumentation.FromAssemblyFile(AssemblyPath, Logger))
            {
                var pageFactory = new CommandLinePageFactory(model, configuration.CommandLineHelp, new DefaultCommandLineHelpPathProvider(), Logger);
                pageFactory.GetPages().Save(
                    configuration.CommandLineHelp.OutputPath,
                    cleanOutputDirectory: true,
                    markdownOptions: configuration.GetSerializationOptions(Logger)
                );
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
