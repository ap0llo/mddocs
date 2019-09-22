using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
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
            using (var assemblyDocumentation = AssemblyDocumentation.FromAssemblyFile(opts.AssemblyPath, logger))
            {
                var pageFactory = new PageFactory(new ApiReference.Pages.DefaultPathProvider(),assemblyDocumentation, logger);
                pageFactory.GetPages().Save(opts.OutputDirectory, cleanOutputDirectory: true);
            }

            return 0;
        }


        private static int OnCommandLineHelpCommand(ILogger logger, CommandLineHelpOptions opts)
        {
            using (var model = ApplicationDocumentation.FromAssemblyFile(opts.AssemblyPath, logger))
            {
                var pageFactory = new CommandLinePageFactory(model, new CommandLineHelp.Pages.DefaultPathProvider(), logger);
                pageFactory.GetPages().Save(opts.OutputDirectory, cleanOutputDirectory: true);
            }

            return 0;
        }

        private static ILogger GetLogger(OptionsBase opts) => new ColoredConsoleLogger(opts.Verbose ? LogLevel.Debug : LogLevel.Information);
    }
}
