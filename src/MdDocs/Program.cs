using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Logging;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var parser = new Parser(opts =>
            {
                opts.CaseInsensitiveEnumValues = true;
                opts.CaseSensitive = false;
                opts.HelpWriter = Console.Out;
            });

            // Parser needs at least two option classes, otherwise it
            // will not include the verb for the commands in the help output.
            return parser
                .ParseArguments<ApiReferenceOptions, CommandLineHelpOptions>(args)
                .MapResult(
                    (ApiReferenceOptions opts) => OnApiReferenceCommand(GetLogger(opts), opts),
                    (CommandLineHelpOptions opts) => OnCommandLineHelpCommand(GetLogger(opts), opts),
                    (IEnumerable<Error> errors) => OnError(errors)); ;
        }

        private static int OnError(IEnumerable<Error> errors)
        {
            // if help or version was requests, the help/version was already
            // written to the output by the parser.
            // There errors can be ignored.
            if (errors.All(e => e is HelpRequestedError || e is HelpVerbRequestedError || e is VersionRequestedError))
            {
                return 0;
            }
            else
            {
                Console.Error.WriteLine("Invalid arguments.");
                return -1;
            }
        }

        private static int OnApiReferenceCommand(ILogger logger, ApiReferenceOptions opts)
        {
            var serializationOptions = GetSerializationOptions(logger, opts);

            if (opts.AssemblyPath == null || String.IsNullOrWhiteSpace(opts.AssemblyPath))
            {
                Console.Error.WriteLine($"Invalid assembly path '{opts.AssemblyPath}'");
                return -1;
            }
            if (opts.OutputDirectory == null || String.IsNullOrWhiteSpace(opts.OutputDirectory))
            {
                Console.Error.WriteLine($"Invalid output directory '{opts.OutputDirectory}'");
                return -1;
            }

            using (var assemblyDocumentation = AssemblyDocumentation.FromAssemblyFile(opts.AssemblyPath, logger))
            {
                var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), assemblyDocumentation, logger);
                pageFactory.GetPages().Save(
                    opts.OutputDirectory,
                    cleanOutputDirectory: true,
                    markdownOptions: serializationOptions);
            }

            return 0;
        }

        private static int OnCommandLineHelpCommand(ILogger logger, CommandLineHelpOptions opts)
        {
            var serializationOptions = GetSerializationOptions(logger, opts);

            if (opts.AssemblyPath == null || String.IsNullOrWhiteSpace(opts.AssemblyPath))
            {
                Console.Error.WriteLine($"Invalid assembly path '{opts.AssemblyPath}'");
                return -1;
            }
            if (opts.OutputDirectory == null || String.IsNullOrWhiteSpace(opts.OutputDirectory))
            {
                Console.Error.WriteLine($"Invalid output directory '{opts.OutputDirectory}'");
                return -1;
            }

            using (var model = ApplicationDocumentation.FromAssemblyFile(opts.AssemblyPath, logger))
            {
                var pageFactory = new CommandLinePageFactory(model, opts, new DefaultCommandLineHelpPathProvider(), logger);
                pageFactory.GetPages().Save(
                    opts.OutputDirectory,
                    cleanOutputDirectory: true,
                    markdownOptions: serializationOptions);

            }

            return 0;
        }

        private static ILogger GetLogger(OptionsBase opts)
        {
            var loggerConfiguration = new SimpleConsoleLoggerConfiguration(
                minimumLogLevel: opts.Verbose ? LogLevel.Debug : LogLevel.Information,
                showCategoryName: false,
                enabledColoredOutput: true);

            return new SimpleConsoleLogger(loggerConfiguration, "");
        }

        private static MdSerializationOptions GetSerializationOptions(ILogger logger, OptionsBase opts)
        {
            var presetName = opts.MarkdownPreset.ToString();

            try
            {
                var preset = MdSerializationOptions.Presets.Get(presetName);
                logger.LogInformation($"Using preset '{presetName}' for generating markdown");
                return preset;
            }
            catch (PresetNotFoundException)
            {
                logger.LogInformation($"Preset '{presetName}' not found. Using default serialization options");
                return MdSerializationOptions.Presets.Default;
            }
        }


    }
}
