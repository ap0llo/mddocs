using System;
using System.IO;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Loaders;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Commands;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Commands
{
    public class CommandLineHelpCommand : ICommand
    {
        private readonly ILogger m_Logger;
        private readonly CommandLineHelpConfiguration m_Configuration;


        public CommandLineHelpCommand(ILogger logger, CommandLineHelpConfiguration configuration)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public bool Execute()
        {
            if (!ValidateConfiguration())
                return false;

            using var assembly = AssemblyReader.ReadFile(m_Configuration.AssemblyPath, m_Logger);

            var loader = new CommandLineParserLoader(m_Logger);

            var model = loader.Load(assembly);

            var pageFactory = new CommandLinePageFactory(model, m_Configuration, new DefaultCommandLineHelpPathProvider(), m_Logger);
            pageFactory.GetPages().Save(
                m_Configuration.OutputPath,
                cleanOutputDirectory: true,
                markdownOptions: m_Configuration.GetSerializationOptions(m_Logger));

            return true;
        }


        private bool ValidateConfiguration()
        {
            var valid = true;

            if (String.IsNullOrWhiteSpace(m_Configuration.OutputPath))
            {
                m_Logger.LogError($"Invalid output directory '{m_Configuration.OutputPath}'");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(m_Configuration.AssemblyPath))
            {
                m_Logger.LogError($"Invalid assembly path '{m_Configuration.AssemblyPath}'");
                valid = false;
            }
            else if (!File.Exists(m_Configuration.AssemblyPath))
            {
                m_Logger.LogError($"Assembly at '{m_Configuration.AssemblyPath}' does not exist.");
                valid = false;
            }

            return valid;
        }
    }
}
