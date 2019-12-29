#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a module.
    /// </summary>
    public sealed class ModuleDocumentation : IDocumentation
    {
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;
        private readonly IDictionary<NamespaceId, NamespaceDocumentation> m_Namespaces;
        private readonly IXmlDocsProvider m_XmlDocsProvider;
        private readonly ILogger m_Logger;

        /// <summary>
        /// Gets the documentation model of the assembly this module is part of.
        /// </summary>
        public AssemblyDocumentation AssemblyDocumentation { get; }

        /// <summary>
        /// Gets the types defined in this module.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }

        /// <summary>
        /// Gets the namespaces defined in this module.
        /// </summary>
        public IReadOnlyCollection<NamespaceDocumentation> Namespaces { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the module.
        /// </summary>
        internal ModuleDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ModuleDocumentation"/>.
        /// </summary>
        /// <param name="assemblyDocumentation">The documentation model of the assembly this module is part of.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the module.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <param name="logger">The logger to use.</param>
        internal ModuleDocumentation(AssemblyDocumentation assemblyDocumentation, ModuleDefinition definition, IXmlDocsProvider xmlDocsProvider, ILogger logger)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));            
            AssemblyDocumentation = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));

            m_Types = new Dictionary<TypeId, TypeDocumentation>();
            m_Namespaces = new Dictionary<NamespaceId, NamespaceDocumentation>();

            foreach (var typeDefinition in Definition.Types.Where(t => t.IsPublic))
            {
                LoadTypeRecursively(typeDefinition, declaringType: null);
            }

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
        }


        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId member)
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



        private void LoadTypeRecursively(TypeDefinition typeDefinition, TypeDocumentation declaringType)
        {
            var typeId = typeDefinition.ToTypeId();
            var namespaceDocumentation = GetNamespaceDocumentation(typeId.Namespace.Name);

            var typeDocumentation = new TypeDocumentation(this, namespaceDocumentation, typeDefinition, m_XmlDocsProvider, m_Logger, declaringType);
            declaringType?.AddNestedType(typeDocumentation);

            m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
            namespaceDocumentation.AddType(typeDocumentation);

            if(typeDefinition.HasNestedTypes)
            {
                foreach(var nestedType in typeDefinition.NestedTypes.Where(x => x.IsNestedPublic))
                {
                    LoadTypeRecursively(nestedType, typeDocumentation);
                }
            }
        }

    }
}
