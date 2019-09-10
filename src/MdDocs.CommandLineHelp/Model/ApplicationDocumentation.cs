using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    //TODO: Handle applications without subcommands
    //TODO: Load data from AssemblyUsageAttribute   
    public sealed class ApplicationDocumentation
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<CommandDocumentation> Commands { get; }


        public ApplicationDocumentation(string name, string version = null, IEnumerable<CommandDocumentation> commands = null)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));
            }

            Name = name;
            Version = version;
            Commands = commands?.OrderBy(x => x.Name)?.ToArray() ?? Array.Empty<CommandDocumentation>();
        }

        private ApplicationDocumentation(AssemblyDefinition definition, ILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            Name = definition
                .GetAttributeOrDefault(Constants.AssemblyTitleAttributeFullName)
                ?.ConstructorArguments?.Single().Value as string;

            if (String.IsNullOrEmpty(Name))
            {
                Name = definition.Name.Name;
            }

            Version = definition
                .GetAttributeOrDefault(Constants.AssemblyInformationalVersionAttribute)
                ?.ConstructorArguments.Single().Value as string;

            if (String.IsNullOrEmpty(Version))
            {
                Version = definition.Name.Version.ToString();
            }

            Commands = LoadCommands(definition, logger);
        }


        public static ApplicationDocumentation FromAssemblyFile(string filePath, ILogger logger)
        {
            // TODO: Share this code with AssemblyDocumentation as far as possible
            var dir = Path.GetDirectoryName(filePath);

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(dir);

            // load assembly
            logger.LogInformation($"Loading assembly from '{filePath}'");
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath, new ReaderParameters() { AssemblyResolver = assemblyResolver });

            using (assemblyDefinition)
            {
                return FromAssemblyDefinition(assemblyDefinition, logger);
            }
        }

        public static ApplicationDocumentation FromAssemblyDefinition(AssemblyDefinition definition, ILogger logger) =>
            new ApplicationDocumentation(definition, logger);


        private IReadOnlyList<CommandDocumentation> LoadCommands(AssemblyDefinition definition, ILogger logger)
        {
            return definition.MainModule.Types
                .WithAttribute(Constants.VerbAttributeFullName)
                .Select(type => CommandDocumentation.FromTypeDefinition(this, type, logger))
                .ToArray();
        }
    }
}
