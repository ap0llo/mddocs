using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class ApplicationDocumentation : IDisposable
    {
        private readonly AssemblyDefinition m_AssemblyDefinition;
        private readonly ILogger m_Logger;


        public IReadOnlyList<CommandDocumentation> Commands { get; }


        private ApplicationDocumentation(AssemblyDefinition assemblyDefinition, ILogger logger)
        {
            m_AssemblyDefinition = assemblyDefinition ?? throw new ArgumentNullException(nameof(assemblyDefinition));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));


            Commands = LoadCommands();
        }


        public void Dispose() => m_AssemblyDefinition.Dispose();


        public static ApplicationDocumentation FromAssemblyFile(string filePath, ILogger logger)
        {
            // TODO: Share this code with AssemblyDocumentation as far as possible
            var dir = Path.GetDirectoryName(filePath);

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(dir);

            // load assembly
            logger.LogInformation($"Loading assembly from '{filePath}'");
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath, new ReaderParameters() { AssemblyResolver = assemblyResolver });

            return new ApplicationDocumentation(assemblyDefinition, logger);

        }


        private IReadOnlyList<CommandDocumentation> LoadCommands()
        {
            var commands = new List<CommandDocumentation>();

            foreach (var type in m_AssemblyDefinition.MainModule.Types)
            {
                if (type.CustomAttributes.Any(x => x.AttributeType.FullName == Constants.VerbAttributeFullName))
                {
                    commands.Add(new CommandDocumentation(type));
                }
            }

            return commands;
        }
    }
}
