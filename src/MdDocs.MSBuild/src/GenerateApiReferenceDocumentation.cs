using Grynwald.MdDocs.ApiReference.Commands;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateApiReferenceDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:apireference:assemblyPaths")]
        public string[] AssemblyPaths => new[] { Assembly.GetFullPath() };

        [ConfigurationValue("mddocs:apireference:outputPath")]
        public string OutputDirectoryPath => OutputDirectory?.GetFullPath() ?? "";


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var configuration = GetConfigurationProvider().GetApiReferenceConfiguration();
            var command = new ApiReferenceCommand(Logger, configuration);
            var success = command.Execute();
            return success && (Log.HasLoggedErrors == false);
        }
    }
}
