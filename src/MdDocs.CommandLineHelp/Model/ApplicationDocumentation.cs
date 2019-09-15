using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    //TODO: Handle applications without subcommands  
    public sealed class ApplicationDocumentation
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<string> Usage { get; }

        public IReadOnlyList<CommandDocumentation> Commands { get; }


        public ApplicationDocumentation(string name, string version = null, IEnumerable<CommandDocumentation> commands = null, IEnumerable<string> usage = null)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));
            }

            Name = name;
            Version = version;
            Commands = commands?.OrderBy(x => x.Name)?.ToArray() ?? Array.Empty<CommandDocumentation>();
            Usage = usage?.ToArray() ?? Array.Empty<string>();
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
            Usage = LoadAssemblyUsage(definition);
        }

     
        public static ApplicationDocumentation FromAssemblyFile(string filePath, ILogger logger)
        {
            using (var assemblyDefinition = AssemblyReader.ReadFile(filePath, logger))
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

        private IReadOnlyList<string> LoadAssemblyUsage(AssemblyDefinition definition)
        {
            var assemblyUsageAttribute = definition.GetAttributeOrDefault(Constants.AssemblyUsageAttributeFullName);
            if (assemblyUsageAttribute != null)
            {
                return assemblyUsageAttribute.ConstructorArguments.Select(x => x.Value).Cast<string>().ToArray();
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
