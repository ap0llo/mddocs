using System;
using System.Collections.Generic;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of an assembly.
    /// </summary>
    public sealed class AssemblyDocumentation : IDisposable, IDocumentation
    {
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;

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
        internal AssemblyDocumentation(AssemblySetDocumentation assemblySet, AssemblyDefinition definition)
        {
            AssemblySet = assemblySet ?? throw new ArgumentNullException(nameof(assemblySet));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            Name = definition.Name.Name;
            Version = definition.GetInformationalVersionOrVersion();

            m_Types = new Dictionary<TypeId, TypeDocumentation>();
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public void Dispose() => Definition.Dispose();

        /// <inheritdoc />
        public IDocumentation? TryGetDocumentation(MemberId member)
        {
            if (member is TypeId typeId && m_Types.TryGetValue(typeId, out var typeDocumentation))
            {
                return typeDocumentation;
            }
            else if (member is TypeMemberId typeMemberId && m_Types.TryGetValue(typeMemberId.DefiningType, out var definingTypeDocumentation))
            {
                return definingTypeDocumentation.TryGetDocumentation(member);
            }
            else
            {
                return AssemblySet.TryGetDocumentation(member);
            }
        }


        /// <summary>
        /// Adds the specified type to the namespace's type list.
        /// </summary>
        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);

    }
}
