using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of an assembly.
    /// </summary>
    public sealed class AssemblyDocumentation : IDisposable, IDocumentation
    {
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;
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
        /// Gets the types defined in this assembly.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }

        /// <summary>
        /// Gets the assembly's definition.
        /// </summary>
        internal AssemblyDefinition Definition { get; }


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

            foreach (var typeDefinition in Definition.MainModule.Types.Where(t => t.IsPublic))
            {
                LoadTypeRecursively(typeDefinition, declaringType: null);
            }

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public void Dispose() => Definition.Dispose();

        /// <inheritdoc />
        public IDocumentation? TryGetDocumentation(MemberId member)
        {
            switch (member)
            {
                case TypeId typeId:
                    if (m_Types.TryGetValue(typeId, out var typeDocumentation))
                    {
                        return typeDocumentation;
                    }
                    break;

                case TypeMemberId typeMemberId:
                    if (m_Types.TryGetValue(typeMemberId.DefiningType, out var definingTypeDocumentation))
                    {
                        return definingTypeDocumentation.TryGetDocumentation(member);
                    }
                    break;

                default:
                    break;
            }

            return AssemblySet.TryGetDocumentation(member);
        }


        private void LoadTypeRecursively(TypeDefinition typeDefinition, TypeDocumentation? declaringType)
        {
            var typeId = typeDefinition.ToTypeId();
            var namespaceDocumentation = AssemblySet.GetOrAddNamespace(typeId.Namespace.Name);

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


    }
}
