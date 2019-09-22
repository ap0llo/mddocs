using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineReference : TaskBase
    {
        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var model = ApplicationDocumentation.FromAssemblyFile(AssemblyPath, Logger);

            var pageFactory = new CommandLinePageFactory(model, new DefaultPathProvider(), Logger);
            var documentSet = pageFactory.GetPages();

            documentSet.Save(OutputDirectoryPath, cleanOutputDirectory: true);

            return Log.HasLoggedErrors == false;
        }
    }
}
