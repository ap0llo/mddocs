using System;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Grynwald.MdDocs.MSBuild
{
    public sealed class GenerateCommandLineReference : Task
    {
        [Required]
        public ITaskItem Assembly { get; set; }

        [Required]
        public ITaskItem OutputDirectory { get; set; }


        public override bool Execute()
        {
            var assemblyPath = Assembly.GetFullPath();
            var outputDirectory = OutputDirectory.GetFullPath();
            var logger = new MSBuildLogger(Log);

            var model = ApplicationDocumentation.FromAssemblyFile(assemblyPath, logger);

            var pageFactory = new CommandLinePageFactory(model, new DefaultPathProvider(), logger);
            var documentSet = pageFactory.GetPages();

            documentSet.Save(outputDirectory, cleanOutputDirectory: true);

            return !Log.HasLoggedErrors;
        }
    }
}
