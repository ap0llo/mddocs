using System;
using System.Collections.Generic;
using System.IO;
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
                    (ApiReferenceOptions opts) => OnApiReferenceCommand(opts),
                    (CommandLineHelpOptions opts) => OnCommandLineHelpCommand(opts),
                    (IEnumerable<Error> errors) => OnError(errors));
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

        private static int OnApiReferenceCommand(ApiReferenceOptions opts)
        {
            var logger = new ColoredConsoleLogger(opts.Verbose ? LogLevel.Debug : LogLevel.Information);

            if (Directory.Exists(opts.OutputDirectory))
            {
                logger.LogInformation($"Cleaning output directory '{opts.OutputDirectory}'");
                Directory.Delete(opts.OutputDirectory, true);
            }

            using (var assemblyDocumentation = AssemblyDocumentation.FromFile(opts.AssemblyPath, logger))
            {
                var factory = new PageFactory(assemblyDocumentation, opts.OutputDirectory, logger);
                factory.SaveAll();
            }

            return 0;
        }


        private static int OnCommandLineHelpCommand(CommandLineHelpOptions opts)
        {
            var logger = new ColoredConsoleLogger(opts.Verbose ? LogLevel.Debug : LogLevel.Information);

            using (var model = ApplicationDocumentation.FromAssemblyFile(opts.AssemblyPath, logger))
            {
                var factory = new CommandLinePageFactory(model, logger);
                var documentSet = factory.GetPages();

                documentSet.Save(opts.OutputDirectory, cleanOutputDirectory: true);
            }

            return 0;            
        }
    }
}
