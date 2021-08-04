using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public sealed class AssemblyDocumentationBuilder
    {
        private readonly Dictionary<string, _AssemblyDocumentation> m_Assemblies = new(StringComparer.OrdinalIgnoreCase);


        public IReadOnlyCollection<_AssemblyDocumentation> Assemblies { get; }


        public AssemblyDocumentationBuilder()
        {
            Assemblies = ReadOnlyCollectionAdapter.Create(m_Assemblies.Values);
        }


        public _AssemblyDocumentation GetOrAddAssembly(string assemblyName, string? assemblyVersion)
        {
            if (String.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentException("Value must not be null or whitespace", nameof(assemblyName));

            if (m_Assemblies.TryGetValue(assemblyName, out var existingAssembly))
            {
                return existingAssembly;
            }

            var assembly = new _AssemblyDocumentation(assemblyName, assemblyVersion);
            m_Assemblies.Add(assemblyName, assembly);

            return assembly;
        }

    }
}
