using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
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

            return parser
                // Add at least 2 option classes, otherwise the command line parser
                // will not include the verb for the commands in the help output
                .ParseArguments<ApiReferenceOptions, DummyOptions>(args)
                .MapResult(
                    (ApiReferenceOptions opts) => ApiReference(opts),
                    (DummyOptions opts) =>
                    {
                        Console.Error.WriteLine("Invalid arguments.");
                        return -1;
                    },
                    (IEnumerable<Error> errors) =>
                    {
                        if (errors.All(e => e is HelpRequestedError || e is HelpVerbRequestedError || e is VersionRequestedError))
                        {
                            return 0;
                        }
                        else
                        {
                            Console.Error.WriteLine("Invalid arguments.");
                            return -1;
                        }
                    });
        }

        private static int ApiReference(ApiReferenceOptions opts)
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
    }
}
