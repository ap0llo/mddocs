using System;
using System.Collections.Generic;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a namespace.
    /// </summary>
    public sealed class _NamespaceDocumentation
    {
        // TODO 2021-08-04: Add types
        private readonly IDictionary<NamespaceId, _NamespaceDocumentation> m_Namespaces = new Dictionary<NamespaceId, _NamespaceDocumentation>();


        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name => NamespaceId.Name;

        /// <summary>
        /// Gets the id of the namespace.
        /// </summary>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        /// Gets the documentation model of the namespace that contains this namespace.
        /// </summary>        
        /// <example>
        /// If this instance is for example the namespace <c>System.Collections</c>, the parent namespace would be <c>System</c>.
        /// </example>
        /// <value>
        /// The documentation model for the parent namespace or <c>null</c> is there is no parent namespace.
        /// </value>
        public _NamespaceDocumentation? ParentNamespace { get; }


        /// <summary>
        /// Gets the namespace's child-namespaces.
        /// </summary>
        public IReadOnlyCollection<_NamespaceDocumentation> Namespaces { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceDocumentation"/>.
        /// </summary>
        internal _NamespaceDocumentation(_NamespaceDocumentation? parentNamespace, NamespaceId namespaceId)
        {
            if (namespaceId is null)
                throw new ArgumentNullException(nameof(namespaceId));

            // parentNamespace must only be null, if the id is the global namespace (which has no parent)
            if (parentNamespace is null && !NamespaceId.GlobalNamespace.Equals(namespaceId))
                throw new ArgumentNullException(nameof(parentNamespace));

            if (parentNamespace is not null)
            {
                if (parentNamespace.NamespaceId.Equals(NamespaceId.GlobalNamespace))
                {
                    if (namespaceId.Name.Contains(".") || String.IsNullOrEmpty(namespaceId.Name))
                    {
                        throw new ArgumentException($"Cannot initialize namespace '{namespaceId}' with parent namespace '{parentNamespace.NamespaceId}'");
                    }
                }
                else if (!namespaceId.Name.StartsWith($"{parentNamespace.Name}."))
                {
                    throw new ArgumentException($"Cannot initialize namespace '{namespaceId}' with parent namespace '{parentNamespace.NamespaceId}'");
                }
            }

            ParentNamespace = parentNamespace;
            NamespaceId = namespaceId;

            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
            //TODO: Support XML docs for namespaces
        }

        /// <inheritdoc />
        public override string ToString() => $"Namespace({NamespaceId})";



        /// <summary>
        /// Adds the specified namespace to the namespace's namespace list.
        /// </summary>
        internal void Add(_NamespaceDocumentation @namespace)
        {
            if (@namespace is null)
                throw new ArgumentNullException(nameof(@namespace));

            if (!ReferenceEquals(@namespace.ParentNamespace, this))
                throw new ArgumentException($"Cannot add namespace '{@namespace.NamespaceId}' as a child of namespace '{NamespaceId}' because the parent namespace if different from the current instance", nameof(@namespace));

            m_Namespaces.Add(@namespace.NamespaceId, @namespace);
        }
    }
}
