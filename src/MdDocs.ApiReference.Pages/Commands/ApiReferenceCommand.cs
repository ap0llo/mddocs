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
        private readonly DocsConfiguration m_Configuration;

        public ApiReferenceCommand(ILogger logger, DocsConfiguration configuration)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public bool Execute()
        {
            if (String.IsNullOrWhiteSpace(m_Configuration.ApiReference.OutputPath))
            {
                m_Logger.LogError($"Invalid output directory '{m_Configuration.ApiReference.OutputPath}'");
                return false;
            }

            if (String.IsNullOrWhiteSpace(m_Configuration.ApiReference.AssemblyPath))
            {
                m_Logger.LogError($"Invalid assembly path '{m_Configuration.ApiReference.AssemblyPath}'");
                return false;
            }

            if (!File.Exists(m_Configuration.ApiReference.AssemblyPath))
            {
                m_Logger.LogError($"Assembly at '{m_Configuration.ApiReference.AssemblyPath}' does not exist.");
                return false;
            }


            using (var assemblyDocumentation = AssemblyDocumentation.FromAssemblyFile(m_Configuration.ApiReference.AssemblyPath, m_Logger))
            {
                var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), assemblyDocumentation, m_Logger);
                pageFactory.GetPages().Save(
                    m_Configuration.ApiReference.OutputPath,
                    cleanOutputDirectory: true,
                    markdownOptions: m_Configuration.GetSerializationOptions(m_Logger));
            }

            return true;
        }
    }
}
