using System;
using System.IO;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class AssemblyDocumentation : IDisposable, IDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;
        private readonly ILogger m_Logger;

        internal AssemblyDefinition Definition { get; }

        public ModuleDocumentation MainModuleDocumentation { get; }



        internal AssemblyDocumentation(AssemblyDefinition definition, IXmlDocsProvider xmlDocsProvider, ILogger logger)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            MainModuleDocumentation = new ModuleDocumentation(this, definition.MainModule, m_XmlDocsProvider, m_Logger);
        }


        public void Dispose()
        {
            Definition.Dispose();
        }

        public IDocumentation TryGetDocumentation(MemberId member) =>
            MainModuleDocumentation.TryGetDocumentation(member);

        public static AssemblyDocumentation FromFile(string filePath) => FromFile(filePath, NullLogger.Instance);

        public static AssemblyDocumentation FromFile(string filePath, ILogger logger)
        {
            logger.LogInformation($"Loading assembly from '{filePath}'");
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);

            var docsFilePath = Path.ChangeExtension(filePath, ".xml");

            IXmlDocsProvider xmlDocsProvider;
            if (File.Exists(docsFilePath))
            {
                xmlDocsProvider = new XmlDocsProvider(docsFilePath, logger);
            }
            else
            {
                logger.LogWarning($"No XML documentation file for assembly found. (Expected at '{docsFilePath}')");
                xmlDocsProvider = NullXmlDocsProvider.Instance;
            }

            return new AssemblyDocumentation(assemblyDefinition, xmlDocsProvider, logger);
        }
    }
}
