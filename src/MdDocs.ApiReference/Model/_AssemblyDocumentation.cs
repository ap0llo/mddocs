using System;
using System.Collections.Generic;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of an assembly.
    /// </summary>
    public sealed class _AssemblyDocumentation
    {
        private readonly Dictionary<TypeId, TypeDocumentation> m_Types = new();

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
        /// Initializes a new instance of <see cref="AssemblyDocumentation"/>.
        /// </summary>
        internal _AssemblyDocumentation(string name, string? version)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            Name = name;
            Version = version;

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }



    }
}
