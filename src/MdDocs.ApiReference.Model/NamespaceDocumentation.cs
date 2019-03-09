using System;
using System.Collections.Generic;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class NamespaceDocumentation : IDocumentation
    {
        private readonly ModuleDocumentation m_ModuleDocumentation;
        private readonly ILogger m_Logger;
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types = new Dictionary<TypeId, TypeDocumentation>();


        public string Name => NamespaceId.Name;

        public NamespaceId NamespaceId { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        internal NamespaceDocumentation(ModuleDocumentation moduleDocumentation, NamespaceId namespaceId, ILogger logger)
        {
            m_ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Logger.LogDebug($"Initializing documentation object for namespace '{namespaceId.Name}'");

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            //TODO: Support XML docs for namespaces
        }


        public IDocumentation TryGetDocumentation(MemberId member) => m_ModuleDocumentation.TryGetDocumentation(member);


        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
    }
}
