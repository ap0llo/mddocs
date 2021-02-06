using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Model;
using Grynwald.Utilities.Collections;
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
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;
        private readonly IDictionary<NamespaceId, NamespaceDocumentation> m_Namespaces;
        private readonly IXmlDocsProvider m_XmlDocsProvider;
        private readonly ILogger m_Logger;

        /// <summary>
        /// The set of all assemblies documentation is being generated for.
        /// </summary>
        public AssemblySetDocumentation AssemblySet { get; }

        /// <summary>
        /// The name of the assembly
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The version of the assembly
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// Gets the assembly's definition.
        /// </summary>
        internal AssemblyDefinition Definition { get; }


        /// <summary>
        /// Gets the types defined in this assembly.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }

        /// <summary>
        /// Gets the namespaces defined in this assembly.
        /// </summary>
        public IReadOnlyCollection<NamespaceDocumentation> Namespaces { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="AssemblyDocumentation"/>.
        /// </summary>
        /// <param name="assemblySet">The set of all assemblies documentation is being generated for.</param>
        /// <param name="definition">The definition of the assembly.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <param name="logger">The logger to use.</param>
        internal AssemblyDocumentation(AssemblySetDocumentation assemblySet, AssemblyDefinition definition, IXmlDocsProvider xmlDocsProvider, ILogger logger)
        {
            AssemblySet = assemblySet ?? throw new ArgumentNullException(nameof(assemblySet));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Name = definition.Name.Name;
            Version = definition.GetInformationalVersionOrVersion();

            m_Types = new Dictionary<TypeId, TypeDocumentation>();
            m_Namespaces = new Dictionary<NamespaceId, NamespaceDocumentation>();

            foreach (var typeDefinition in Definition.MainModule.Types.Where(t => t.IsPublic))
            {
                LoadTypeRecursively(typeDefinition, declaringType: null);
            }

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
        }


        public void Dispose() => Definition.Dispose();

        /// <inheritdoc />
        public IDocumentation? TryGetDocumentation(MemberId member)
        {
            switch (member)
            {
                case TypeId typeId:
                    return m_Types.GetValueOrDefault(typeId);

                case TypeMemberId typeMemberId:
                    return m_Types.GetValueOrDefault(typeMemberId.DefiningType)?.TryGetDocumentation(member);

                case NamespaceId namespaceId:
                    return m_Namespaces.GetValueOrDefault(namespaceId);

                default:
                    return null;
            }
        }


        private void LoadTypeRecursively(TypeDefinition typeDefinition, TypeDocumentation? declaringType)
        {
            var typeId = typeDefinition.ToTypeId();
            var namespaceDocumentation = GetNamespaceDocumentation(typeId.Namespace.Name);

            var typeDocumentation = new TypeDocumentation(this, namespaceDocumentation, typeDefinition, m_XmlDocsProvider, m_Logger, declaringType);
            declaringType?.AddNestedType(typeDocumentation);

            m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
            namespaceDocumentation.AddType(typeDocumentation);

            if (typeDefinition.HasNestedTypes)
            {
                foreach (var nestedType in typeDefinition.NestedTypes.Where(x => x.IsNestedPublic))
                {
                    LoadTypeRecursively(nestedType, typeDocumentation);
                }
            }
        }

        private NamespaceDocumentation GetNamespaceDocumentation(string namespaceName)
        {
            var namespaceId = new NamespaceId(namespaceName);

            if (m_Namespaces.ContainsKey(namespaceId))
            {
                return m_Namespaces[namespaceId];
            }

            var names = namespaceName.Split('.');

            var parentNamespace = names.Length > 1
                ? GetNamespaceDocumentation(names.Take(names.Length - 1).JoinToString("."))
                : null;

            var newNamespace = new NamespaceDocumentation(this, parentNamespace, namespaceId, m_Logger);
            m_Namespaces.Add(namespaceId, newNamespace);

            parentNamespace?.AddNamespace(newNamespace);

            return newNamespace;
        }

    }
}
