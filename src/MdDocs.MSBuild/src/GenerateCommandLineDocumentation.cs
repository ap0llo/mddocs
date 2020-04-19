using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:commandlinehelp:includeVersion")]
        public bool IncludeVersion { get; set; } = true;


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var serializationOptions = GetSerializationOptions();

            //TODO: Load a configuration file
            var configuration = DocsConfigurationLoader.GetConfiguration("", this);

            using (var model = ApplicationDocumentation.FromAssemblyFile(AssemblyPath, Logger))
            {
                var pageFactory = new CommandLinePageFactory(model, configuration.CommandLineHelp, new DefaultCommandLineHelpPathProvider(), Logger);
                pageFactory.GetPages().Save(OutputDirectoryPath, cleanOutputDirectory: true, markdownOptions: serializationOptions);
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
