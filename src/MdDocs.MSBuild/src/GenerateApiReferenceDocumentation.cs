using System.IO;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.Configuration;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateApiReferenceDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:apireference:assemblyPath")]
        public string AssemblyPath => Assembly.GetFullPath();

        [ConfigurationValue("mddocs:apireference:outputPath")]
        public string OutputDirectoryPath => OutputDirectory?.GetFullPath() ?? "";


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            if (Directory.Exists(OutputDirectoryPath))
            {
                Logger.LogInformation($"Cleaning output directory '{OutputDirectoryPath}'");
                Directory.Delete(OutputDirectoryPath, true);
            }


            var configuration = LoadConfiguration();

            using (var assemblyDocumentation = AssemblyDocumentation.FromAssemblyFile(AssemblyPath, Logger))
            {
                var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), assemblyDocumentation, Logger);
                pageFactory.GetPages().Save(
                    configuration.ApiReference.OutputPath,
                    cleanOutputDirectory: true,
                    markdownOptions: configuration.GetSerializationOptions(Logger));
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
