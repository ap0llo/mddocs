﻿using Grynwald.MdDocs.CommandLineHelp.Commands;
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


        [ConfigurationValue("mddocs:commandlinehelp:markdownPreset")]
        public string? MarkdownPreset { get; set; }

        public override bool Execute()
        {
            if (!ValidateParameters())
                return false;

            var configuration = LoadConfiguration();
            var command = new CommandLineHelpCommand(Logger, configuration.CommandLineHelp);
            var success = command.Execute();
            return success && (Log.HasLoggedErrors == false);
        }
    }
}
