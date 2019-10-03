using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineDocumentation : TaskBase, ICommandLinePageOptions
    {
        public bool IncludeVersion { get; set; } = true;


        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var serializationOptions = GetSerializationOptions();

            using (var model = ApplicationDocumentation.FromAssemblyFile(AssemblyPath, Logger))
            {
                var pageFactory = new CommandLinePageFactory(model, this, new DefaultCommandLineHelpPathProvider(), Logger);
                pageFactory.GetPages().Save(OutputDirectoryPath, cleanOutputDirectory: true, markdownOptions: serializationOptions);
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
