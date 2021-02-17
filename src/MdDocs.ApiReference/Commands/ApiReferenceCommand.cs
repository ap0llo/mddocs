using System;
using System.IO;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Templates;
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
            if (!ValidateConfiguration())
                return false;

            using (var assemblySet = AssemblySetDocumentation.FromAssemblyFiles(m_Configuration.AssemblyPaths, m_Logger))
            {
                ApiReferenceTemplateProvider
                    .GetTemplate(m_Logger, m_Configuration)
                    .Render(assemblySet)
                    .Save(
                        m_Configuration.OutputPath,
                        cleanOutputDirectory: true,
                        markdownOptions: m_Configuration.Template.Default.GetSerializationOptions(m_Logger)
                    );
            }

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

            if (m_Configuration.AssemblyPaths is null || !m_Configuration.AssemblyPaths.Any())
            {
                m_Logger.LogError($"No assembly paths specified.");
                valid = false;
            }
            else
            {
                foreach (var assemblyPath in m_Configuration.AssemblyPaths)
                {
                    if (String.IsNullOrWhiteSpace(assemblyPath))
                    {
                        m_Logger.LogError($"Invalid assembly path '{assemblyPath}'");
                        valid = false;
                    }
                    else if (!File.Exists(assemblyPath))
                    {
                        m_Logger.LogError($"Assembly at '{assemblyPath}' does not exist.");
                        valid = false;
                    }
                }
            }


            return valid;
        }
    }
}
