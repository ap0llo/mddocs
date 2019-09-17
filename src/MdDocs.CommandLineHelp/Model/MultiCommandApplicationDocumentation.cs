using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a application with multiple subcommands.
    /// </summary>   
    public sealed class MultiCommandApplicationDocumentation : ApplicationDocumentationBase
    {
        public IReadOnlyList<CommandDocumentation> Commands { get; }


        public MultiCommandApplicationDocumentation(string name, string version = null, IEnumerable<CommandDocumentation> commands = null, IEnumerable<string> usage = null)
            : base(name, version, usage)
        {
            Commands = commands?.OrderBy(x => x.Name)?.ToArray() ?? Array.Empty<CommandDocumentation>();
        }

        private MultiCommandApplicationDocumentation(AssemblyDefinition definition, ILogger logger): base(
                  name: LoadApplicationName(definition ?? throw new ArgumentNullException(nameof(definition))),
                  version: LoadApplicationVersion(definition ?? throw new ArgumentNullException(nameof(definition))),
                  usage: LoadAssemblyUsage(definition))
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            Commands = LoadCommands(definition, logger);
        }

     

        public static MultiCommandApplicationDocumentation FromAssemblyDefinition(AssemblyDefinition definition, ILogger logger) =>
            new MultiCommandApplicationDocumentation(definition, logger);


        private IReadOnlyList<CommandDocumentation> LoadCommands(AssemblyDefinition definition, ILogger logger)
        {
            return definition.MainModule.Types
                .Where(x => !x.IsAbstract)
                .WithAttribute(Constants.VerbAttributeFullName)
                .Select(type => CommandDocumentation.FromTypeDefinition(this, type, logger))
                .ToArray();
        }

    }
}
