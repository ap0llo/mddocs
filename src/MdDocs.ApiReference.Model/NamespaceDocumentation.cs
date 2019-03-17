using System;
using System.Collections.Generic;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a namespace.
    /// </summary>
    public sealed class NamespaceDocumentation : IDocumentation
    {
        private readonly ModuleDocumentation m_ModuleDocumentation;
        private readonly ILogger m_Logger;
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types = new Dictionary<TypeId, TypeDocumentation>();
        private readonly IDictionary<NamespaceId, NamespaceDocumentation> m_Namespaces = new Dictionary<NamespaceId, NamespaceDocumentation>();


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
        public NamespaceDocumentation ParentNamespaceDocumentation { get; }


        /// <summary>
        /// Gets the types defined in this namespace.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        /// <summary>
        /// Gets the namespace's child-namespaces.
        /// </summary>
        public IReadOnlyCollection<NamespaceDocumentation> Namespaces { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceDocumentation"/>.
        /// </summary>
        /// <param name="moduleDocumentation">The documentation model of the module this namespace is defined in.</param>
        /// <param name="parentNamespaceDocumentation">The documentation model of the namespace that contains this namespace.</param>
        /// <param name="namespaceId">The id of the namespace.</param>
        /// <param name="logger">The logger to use.</param>
        internal NamespaceDocumentation(ModuleDocumentation moduleDocumentation, NamespaceDocumentation parentNamespaceDocumentation, NamespaceId namespaceId, ILogger logger)
        {
            m_ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            ParentNamespaceDocumentation = parentNamespaceDocumentation;
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Logger.LogDebug($"Initializing documentation object for namespace '{namespaceId.Name}'");

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
            //TODO: Support XML docs for namespaces
        }


        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId member) => m_ModuleDocumentation.TryGetDocumentation(member);


        /// <summary>
        /// Adds the specified type to the namespace's type list.
        /// </summary>
        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);

        /// <summary>
        /// Adds the specified namespace to the namespace's namespace list.
        /// </summary>
        internal void AddNamespace(NamespaceDocumentation namespaceDocumentation) =>
            m_Namespaces.Add(namespaceDocumentation.NamespaceId, namespaceDocumentation);
    }
}
