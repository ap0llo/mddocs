using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IDictionary<string, AssemblyDocumentation> m_Assemblies;
        private readonly IDictionary<TypeId, AssemblyDocumentation> m_AssembliesByType;
        private readonly IDictionary<NamespaceId, NamespaceDocumentation> m_Namespaces;
        private readonly ILogger m_Logger;


        /// <summary>
        /// Gets the assemblies in the assembly set
        /// </summary>
        public IReadOnlyCollection<AssemblyDocumentation> Assemblies { get; }

        /// <summary>
        /// Gets the namespaces defined in any of the assemblies in the assembly set.
        /// </summary>
        public IReadOnlyCollection<NamespaceDocumentation> Namespaces { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="AssemblySetDocumentation"/>
        /// </summary>
        private AssemblySetDocumentation(IReadOnlyList<AssemblyDefinition> assemblyDefinitions, IReadOnlyList<IXmlDocsProvider> xmlDocsProviders, ILogger logger)
        {
            if (assemblyDefinitions is null)
                throw new ArgumentNullException(nameof(assemblyDefinitions));

            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Assemblies = new Dictionary<string, AssemblyDocumentation>(StringComparer.OrdinalIgnoreCase);
            m_AssembliesByType = new Dictionary<TypeId, AssemblyDocumentation>();
            m_Namespaces = new Dictionary<NamespaceId, NamespaceDocumentation>();


            Assemblies = ReadOnlyCollectionAdapter.Create(m_Assemblies.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);

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
            return member switch
            {
                TypeId typeId => m_AssembliesByType.GetValueOrDefault(typeId)?.TryGetDocumentation(member),
                TypeMemberId typeMemberId => m_AssembliesByType.GetValueOrDefault(typeMemberId.DefiningType)?.TryGetDocumentation(member),
                NamespaceId namespaceId => m_Namespaces.GetValueOrDefault(namespaceId),
                _ => null
            };
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


        internal NamespaceDocumentation GetOrAddNamespace(string namespaceName)
        {
            var namespaceId = new NamespaceId(namespaceName);

            if (m_Namespaces.ContainsKey(namespaceId))
            {
                return m_Namespaces[namespaceId];
            }

            var names = namespaceName.Split('.');

            var parentNamespace = names.Length > 1
                ? GetOrAddNamespace(names.Take(names.Length - 1).JoinToString("."))
                : null;

            var newNamespace = new NamespaceDocumentation(this, parentNamespace, namespaceId, m_Logger);
            m_Namespaces.Add(namespaceId, newNamespace);

            parentNamespace?.AddNamespace(newNamespace);

            return newNamespace;
        }

        private void LoadAssemblies(IReadOnlyList<AssemblyDefinition> assemblyDefinitions, IReadOnlyList<IXmlDocsProvider> xmlDocsProviders)
        {
            if (assemblyDefinitions.Count != xmlDocsProviders.Count)
                throw new ArgumentException("Mismatch between number of assembly definitions and XML docs providers");

            foreach (var (index, assemblyDefinition) in assemblyDefinitions.WithIndex())
            {
                AssemblyDocumentation assemblyDocumentation;
                try
                {
                    assemblyDocumentation = new AssemblyDocumentation(this, assemblyDefinition, xmlDocsProviders[index], m_Logger);
                }
                catch (DuplicateTypeException ex)
                {
                    throw new InvalidAssemblySetException($"Type '{ex.TypeName}' exists in multiple assemblies.");
                }

                if (m_Assemblies.ContainsKey(assemblyDocumentation.Name))
                {
                    throw new InvalidAssemblySetException($"Assembly set contains multiple assemblies named '{assemblyDocumentation.Name}'");
                }

                foreach (var type in assemblyDocumentation.Types)
                {
                    if (m_AssembliesByType.ContainsKey(type.TypeId))
                    {
                        throw new InvalidAssemblySetException($"Type '{type.Definition.FullName}' exists in multiple assemblies.");
                    }
                }

                m_Assemblies.Add(assemblyDocumentation.Name, assemblyDocumentation);
                foreach (var type in assemblyDocumentation.Types)
                {
                    m_AssembliesByType.Add(type.TypeId, assemblyDocumentation);
                }
            }
        }
    }
}
