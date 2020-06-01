using Grynwald.MdDocs.ApiReference.Commands;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateApiReferenceDocumentation : TaskBase
    {
        [ConfigurationValue("mddocs:apireference:assemblyPath")]
        public string AssemblyPath => Assembly.GetFullPath();

        [ConfigurationValue("mddocs:apireference:outputPath")]
        public string OutputDirectoryPath => OutputDirectory?.GetFullPath() ?? "";

        [ConfigurationValue("mddocs:apireference:markdownPreset")]
        public string? MarkdownPreset { get; set; }


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var configuration = LoadConfiguration();
            var command = new ApiReferenceCommand(Logger, configuration);
            var success = command.Execute();
            return success && (Log.HasLoggedErrors == false);
        }
    }
}
