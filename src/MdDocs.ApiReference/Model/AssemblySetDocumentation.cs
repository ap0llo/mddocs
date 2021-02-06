using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for a set of assemblies
    /// </summary>
    public sealed class AssemblySetDocumentation : IDocumentation, IDisposable
    {
        private readonly Dictionary<string, AssemblyDocumentation> m_Assemblies;
        private readonly Dictionary<TypeId, AssemblyDocumentation> m_Types;
        private readonly ILogger m_Logger;


        /// <summary>
        /// Gets the assemblies in the assembly set
        /// </summary>
        public IReadOnlyCollection<AssemblyDocumentation> Assemblies { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="AssemblySetDocumentation"/>
        /// </summary>
        private AssemblySetDocumentation(IReadOnlyList<AssemblyDefinition> assemblyDefinitions, IReadOnlyList<IXmlDocsProvider> xmlDocsProviders, ILogger logger)
        {
            if (assemblyDefinitions is null)
                throw new ArgumentNullException(nameof(assemblyDefinitions));

            m_Assemblies = new(StringComparer.OrdinalIgnoreCase);
            m_Types = new();

            Assemblies = ReadOnlyCollectionAdapter.Create(m_Assemblies.Values);
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            LoadAssemblies(assemblyDefinitions, xmlDocsProviders);
        }


        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var assembly in Assemblies)
            {
                assembly.Dispose();
            }
        }

        /// <inheritdoc />
        public IDocumentation? TryGetDocumentation(MemberId member)
        {
            foreach (var assembly in Assemblies)
            {
                var resolved = assembly.TryGetDocumentation(member);
                if (resolved is not null)
                    return resolved;
            }

            return null;
        }


        public static AssemblySetDocumentation FromAssemblyFiles(IEnumerable<string> filePaths) =>
            FromAssemblyFiles(filePaths, NullLogger.Instance);

        /// <summary>
        /// Creates a new <see cref="AssemblySetDocumentation"/> from the specified assembly files
        /// </summary>
        public static AssemblySetDocumentation FromAssemblyFiles(IEnumerable<string> filePaths, ILogger logger)
        {
            var assemblyDefinitions = new List<AssemblyDefinition>();
            var xmlDocsProviders = new List<IXmlDocsProvider>();

            foreach (var filePath in filePaths)
            {
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);
                assemblyDefinitions.Add(assemblyDefinition);

                // loads XML documentation comments if the documentation file exists
                IXmlDocsProvider docs;
                var docsFilePath = Path.ChangeExtension(filePath, ".xml");
                if (File.Exists(docsFilePath))
                {
                    docs = new XmlDocsProvider(assemblyDefinition, docsFilePath, logger);
                }
                else
                {
                    logger.LogWarning($"No XML documentation file for assembly found. (Expected at '{docsFilePath}')");
                    docs = NullXmlDocsProvider.Instance;
                }

                xmlDocsProviders.Add(docs);
            }

            return new AssemblySetDocumentation(assemblyDefinitions, xmlDocsProviders, logger);
        }

        public static AssemblySetDocumentation FromAssemblyDefinitions(params AssemblyDefinition[] assemblyDefinitions) =>
            FromAssemblyDefinitions(assemblyDefinitions, NullLogger.Instance);

        public static AssemblySetDocumentation FromAssemblyDefinitions(IReadOnlyList<AssemblyDefinition> assemblyDefinitions, ILogger logger)
        {
            return new AssemblySetDocumentation(
                assemblyDefinitions,
                assemblyDefinitions.Select(x => NullXmlDocsProvider.Instance).ToList(),
                logger
            );
        }


        private void LoadAssemblies(IReadOnlyList<AssemblyDefinition> assemblyDefinitions, IReadOnlyList<IXmlDocsProvider> xmlDocsProviders)
        {
            if (assemblyDefinitions.Count != xmlDocsProviders.Count)
                throw new ArgumentException("Mismatch between number of assembly definitions and XML docs providers");

            foreach (var (index, assemblyDefinition) in assemblyDefinitions.WithIndex())
            {
                var assemblyDocumentation = new AssemblyDocumentation(this, assemblyDefinition, xmlDocsProviders[index], m_Logger);

                if (m_Assemblies.ContainsKey(assemblyDocumentation.Name))
                {
                    throw new InvalidAssemblySetException($"Assembly set contains multiple assemblies named '{assemblyDocumentation.Name}'");
                }

                foreach (var type in assemblyDocumentation.Types)
                {
                    if (m_Types.ContainsKey(type.TypeId))
                    {
                        throw new InvalidAssemblySetException($"Type '{type.Definition.FullName}' exists in multiple assemblies.");
                    }
                }

                m_Assemblies.Add(assemblyDocumentation.Name, assemblyDocumentation);
                foreach (var type in assemblyDocumentation.Types)
                {
                    m_Types.Add(type.TypeId, assemblyDocumentation);
                }
            }
        }
    }
}
