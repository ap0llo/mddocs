using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for a set of assemblies
    /// </summary>
    public sealed class _AssemblySetDocumentation
    {
        private readonly IDictionary<string, _AssemblyDocumentation> m_Assemblies;
        private readonly IDictionary<NamespaceId, _NamespaceDocumentation> m_Namespaces;
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;


        /// <summary>
        /// Gets the assemblies in the assembly set
        /// </summary>
        public IReadOnlyCollection<_AssemblyDocumentation> Assemblies { get; }

        /// <summary>
        /// Gets the namespaces defined in any of the assemblies in the assembly set.
        /// </summary>
        public IReadOnlyCollection<_NamespaceDocumentation> Namespaces { get; }

        /// <summary>
        /// Gets all the types defined in any assembly in the assembly set.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="AssemblySetDocumentation"/>
        /// </summary>
        public _AssemblySetDocumentation(IEnumerable<_AssemblyDocumentation> assemblies, IEnumerable<_NamespaceDocumentation> namespaces, IEnumerable<TypeDocumentation> types)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            if (namespaces is null)
                throw new ArgumentNullException(nameof(namespaces));

            if (types is null)
                throw new ArgumentNullException(nameof(types));

            var duplicateAssemblies = assemblies.DuplicatesBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            if (duplicateAssemblies.Any())
            {
                throw new InvalidAssemblySetException($"Assembly set cannot contain multiple assemblies named {duplicateAssemblies.First()}");
            }

            m_Assemblies = assemblies.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            m_Namespaces = namespaces.ToDictionary(x => x.NamespaceId);
            m_Types = types.ToDictionary(x => x.TypeId);

            Assemblies = ReadOnlyCollectionAdapter.Create(m_Assemblies.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }
    }
}
