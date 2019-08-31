using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class ApplicationDocumentation
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<CommandDocumentation> Commands { get; }


        public ApplicationDocumentation(string name, string version = null, IEnumerable<CommandDocumentation> commands = null)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            Name = name;
            Version = version;
            Commands = commands?.ToArray() ?? Array.Empty<CommandDocumentation>();
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

            using(assemblyDefinition)
            {
                return FromAssemblyDefinition(assemblyDefinition, logger);
            }
        }

        public static ApplicationDocumentation FromAssemblyDefinition(AssemblyDefinition definition, ILogger logger)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            var name = definition.CustomAttributes
                .SingleOrDefault(a => a.AttributeType.FullName == Constants.AssemblyTitleAttributeFullName)
                ?.ConstructorArguments?.Single().Value as string;

            if(String.IsNullOrEmpty(name))
            {
                name = definition.Name.Name;
            }

            var version = definition.CustomAttributes
                .SingleOrDefault(a => a.AttributeType.FullName == Constants.AssemblyInformationalVersionAttribute)
                ?.ConstructorArguments.Single().Value as string;


            if(String.IsNullOrEmpty(version))
            {
                version = definition.Name.Version.ToString();
            }

            var commands = LoadCommands(definition, logger);
            return new ApplicationDocumentation(name: name, version: version, commands: commands);
        }


        private static IReadOnlyList<CommandDocumentation> LoadCommands(AssemblyDefinition definition, ILogger logger)
        {
            var commands = new List<CommandDocumentation>();

            foreach (var type in definition.MainModule.Types)
            {
                if (type.CustomAttributes.Any(x => x.AttributeType.FullName == Constants.VerbAttributeFullName))
                {
                    commands.Add(CommandDocumentation.FromTypeDefinition(type, logger));
                }
            }

            return commands;
        }
    }
}
