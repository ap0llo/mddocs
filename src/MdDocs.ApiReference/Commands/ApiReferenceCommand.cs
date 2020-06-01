using System;
using System.IO;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Commands;
using Grynwald.MdDocs.Common.Configuration;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Commands
{
    public class ApiReferenceCommand : ICommand
    {
        private readonly ILogger m_Logger;
        private readonly ApiReferenceConfiguration m_Configuration;

        public ApiReferenceCommand(ILogger logger, ApiReferenceConfiguration configuration)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public bool Execute()
        {
            if (String.IsNullOrWhiteSpace(m_Configuration.OutputPath))
            {
                m_Logger.LogError($"Invalid output directory '{m_Configuration.OutputPath}'");
                return false;
            }

            if (String.IsNullOrWhiteSpace(m_Configuration.AssemblyPath))
            {
                m_Logger.LogError($"Invalid assembly path '{m_Configuration.AssemblyPath}'");
                return false;
            }

            if (!File.Exists(m_Configuration.AssemblyPath))
            {
                m_Logger.LogError($"Assembly at '{m_Configuration.AssemblyPath}' does not exist.");
                return false;
            }


            using (var assemblyDocumentation = AssemblyDocumentation.FromAssemblyFile(m_Configuration.AssemblyPath, m_Logger))
            {
                var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), assemblyDocumentation, m_Logger);
                pageFactory.GetPages().Save(
                    m_Configuration.OutputPath,
                    cleanOutputDirectory: true,
                    markdownOptions: m_Configuration.GetSerializationOptions(m_Logger));
            }

            return true;
        }
    }
}
