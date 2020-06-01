using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Grynwald.MdDocs.ApiReference.Commands;
using Grynwald.MdDocs.CommandLineHelp.Commands;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.Logging;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs
{
    internal static class Program
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
            var success = parser
                .ParseArguments<ApiReferenceOptions, CommandLineHelpOptions>(args)
                .MapResult(
                    (ApiReferenceOptions opts) => OnApiReferenceCommand(GetLogger(opts), opts),
                    (CommandLineHelpOptions opts) => OnCommandLineHelpCommand(GetLogger(opts), opts),
                    (IEnumerable<Error> errors) => OnError(errors));

            return success ? 0 : 1;

        }

        private static bool OnError(IEnumerable<Error> errors)
        {
            // if help or version was requests, the help/version was already
            // written to the output by the parser.
            // There errors can be ignored.
            if (errors.All(e => e is HelpRequestedError || e is HelpVerbRequestedError || e is VersionRequestedError))
            {
                return true;
            }
            else
            {
                Console.Error.WriteLine("Invalid arguments.");
                return false;
            }
        }

        private static bool OnApiReferenceCommand(ILogger logger, ApiReferenceOptions opts)
        {
            var configuration = LoadConfiguration(opts);
            var command = new ApiReferenceCommand(logger, configuration);
            return command.Execute();

        }

        private static bool OnCommandLineHelpCommand(ILogger logger, CommandLineHelpOptions opts)
        {
            var configuration = LoadConfiguration(opts);
            var command = new CommandLineHelpCommand(logger, configuration);
            return command.Execute();
        }

        private static ILogger GetLogger(OptionsBase opts)
        {
            var loggerConfiguration = new SimpleConsoleLoggerConfiguration(
                minimumLogLevel: opts.Verbose ? LogLevel.Debug : LogLevel.Information,
                showCategoryName: false,
                enabledColoredOutput: true);

            return new SimpleConsoleLogger(loggerConfiguration, "");
        }

        private static DocsConfiguration LoadConfiguration(OptionsBase commandlineParameters) =>
            DocsConfigurationLoader.GetConfiguration(commandlineParameters.ConfigurationFilePath ?? "", commandlineParameters);

    }
}
