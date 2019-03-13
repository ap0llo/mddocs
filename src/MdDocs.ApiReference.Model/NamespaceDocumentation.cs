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


        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name => NamespaceId.Name;

        /// <summary>
        /// Gets the id of the namespace.
        /// </summary>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        /// Gets the types defined in this namespace.
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceDocumentation"/>.
        /// </summary>
        /// <param name="moduleDocumentation">The documentation model of the module this namespace is defined in.</param>
        /// <param name="namespaceId">The id of the namespace.</param>
        /// <param name="logger">The logger to use.</param>
        internal NamespaceDocumentation(ModuleDocumentation moduleDocumentation, NamespaceId namespaceId, ILogger logger)
        {
            m_ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Logger.LogDebug($"Initializing documentation object for namespace '{namespaceId.Name}'");

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            //TODO: Support XML docs for namespaces
        }


        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId member) => m_ModuleDocumentation.TryGetDocumentation(member);


        /// <summary>
        /// Adds the specified type to the namespace's type list.
        /// </summary>
        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
    }
}
