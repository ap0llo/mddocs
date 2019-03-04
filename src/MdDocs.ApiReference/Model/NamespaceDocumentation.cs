using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class NamespaceDocumentation : IDocumentation
    {
        private readonly ModuleDocumentation m_ModuleDocumentation;
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types = new Dictionary<TypeId, TypeDocumentation>();


        public string Name => NamespaceId.Name;

        public NamespaceId NamespaceId { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        internal NamespaceDocumentation(ModuleDocumentation moduleDocumentation, NamespaceId namespaceId)
        {            
            m_ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
            //TODO: Namespace XML docs
        }


        public IDocumentation TryGetDocumentation(MemberId member) => m_ModuleDocumentation.TryGetDocumentation(member);


        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
    }
}
