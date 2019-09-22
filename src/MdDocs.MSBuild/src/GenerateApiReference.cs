using System.IO;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateApiReference : TaskBase
    {
        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            if (Directory.Exists(OutputDirectoryPath))
            {
                Logger.LogInformation($"Cleaning output directory '{OutputDirectoryPath}'");
                Directory.Delete(OutputDirectoryPath, true);
            }

            //TODO: Make usage of ApplicationDocumentation and AssemblyDocumentation consistent
            using (var assemblyDocumentation = AssemblyDocumentation.FromFile(AssemblyPath, Logger))
            {
                var factory = new PageFactory(assemblyDocumentation, OutputDirectoryPath, Logger);
                factory.SaveAll();
            }

            return Log.HasLoggedErrors == false;
        }
    }
}
