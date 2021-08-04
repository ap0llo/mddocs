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
        private readonly Dictionary<NamespaceId, _NamespaceDocumentation> m_Namespaces = new();
        private readonly Dictionary<TypeId, _TypeDocumentation> m_Types = new();


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
        /// Gets the types in the namespace
        /// </summary>
        public IReadOnlyCollection<_TypeDocumentation> Types { get; }


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
                        throw new InconsistentModelException($"Cannot initialize namespace '{namespaceId}' with parent namespace '{parentNamespace.NamespaceId}'");
                    }
                }
                else if (!namespaceId.Name.StartsWith($"{parentNamespace.Name}."))
                {
                    throw new InconsistentModelException($"Cannot initialize namespace '{namespaceId}' with parent namespace '{parentNamespace.NamespaceId}'");
                }
            }

            ParentNamespace = parentNamespace;
            NamespaceId = namespaceId;

            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
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
                throw new InconsistentModelException($"Cannot add namespace '{@namespace.NamespaceId}' as a child of namespace '{NamespaceId}' because the parent namespace is different from the current instance");

            m_Namespaces.Add(@namespace.NamespaceId, @namespace);
        }


        /// <summary>
        /// Adds the specified type to the namespace
        /// </summary>
        internal void Add(_TypeDocumentation type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (m_Types.ContainsKey(type.TypeId))
                throw new DuplicateItemException($"Type '{type.DisplayName}' already exists");

            if (!ReferenceEquals(type.Namespace, this))
                throw new InconsistentModelException($"Cannot add type '{type.TypeId}' to namespace '{NamespaceId}' because the type's namespace is different from the current instance");


            m_Types.Add(type.TypeId, type);
        }

    }
}
