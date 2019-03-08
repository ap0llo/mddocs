using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;

namespace Grynwald.MdDocs
{
    class Program
    {
        static int Main(string[] args)
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

        static int ApiReference(ApiReferenceOptions opts)
        {
            if (Directory.Exists(opts.OutputDirectory))
                Directory.Delete(opts.OutputDirectory, true);

            using (var assemblyDocumentation = AssemblyDocumentation.FromFile(opts.AssemblyPath))
            {
                var factory = new PageFactory(assemblyDocumentation, opts.OutputDirectory);
                factory.SaveAll();
            }

            return 0;
        }
    }
}
