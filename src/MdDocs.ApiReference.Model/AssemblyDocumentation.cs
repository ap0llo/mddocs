using System;
using System.IO;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of an assembly.
    /// </summary>
    public sealed class AssemblyDocumentation : IDisposable, IDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;
        private readonly ILogger m_Logger;

        /// <summary>
        /// Gets the assembly's definition.
        /// </summary>
        internal AssemblyDefinition Definition { get; }

        /// <summary>
        /// Gets the documentation model for the assembly's main module
        /// </summary>
        public ModuleDocumentation MainModuleDocumentation { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="AssemblyDocumentation"/>.
        /// </summary>
        /// <param name="definition">The definition of the assembly.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <param name="logger">The logger to use.</param>
        internal AssemblyDocumentation(AssemblyDefinition definition, IXmlDocsProvider xmlDocsProvider, ILogger logger)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            MainModuleDocumentation = new ModuleDocumentation(this, definition.MainModule, m_XmlDocsProvider, m_Logger);
        }


        public void Dispose() => Definition.Dispose();

        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId member) =>
            MainModuleDocumentation.TryGetDocumentation(member);

        /// <summary>
        /// Loads the documentation model from an assembly file.
        /// </summary>
        /// <param name="filePath">The file of the assembly to load.</param>
        /// <returns>Returns a new instance of <see cref="AssemblyDocumentation"/> that provides documentation for the specified assembly.</returns>
        public static AssemblyDocumentation FromFile(string filePath) => FromFile(filePath, NullLogger.Instance);

        /// <summary>
        /// Loads the documentation model from an assembly file.
        /// </summary>
        /// <param name="filePath">The file of the assembly to load.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>Returns a new instance of <see cref="AssemblyDocumentation"/> that provides documentation for the specified assembly.</returns>
        public static AssemblyDocumentation FromFile(string filePath, ILogger logger)
        {
            // load assembly
            var assemblyDefinition = AssemblyReader.ReadFile(filePath, logger);

            // loads XML documentation comments if the documentation file exists
            IXmlDocsProvider xmlDocsProvider;
            var docsFilePath = Path.ChangeExtension(filePath, ".xml");
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
