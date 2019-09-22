using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineDocumentation : TaskBase
    {
        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            using (var model = ApplicationDocumentation.FromAssemblyFile(AssemblyPath, Logger))
            {
                var pageFactory = new CommandLinePageFactory(model, new DefaultPathProvider(), Logger);
                pageFactory.GetPages().Save(OutputDirectoryPath, cleanOutputDirectory: true); 
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
