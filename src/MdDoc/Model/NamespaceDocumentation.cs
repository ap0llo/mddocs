using System;
using System.Collections.Generic;

namespace MdDoc.Model
{
    public class NamespaceDocumentation : IDocumentation
    {
        private readonly ModuleDocumentation m_ModuleDocumentation;
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types = new Dictionary<TypeId, TypeDocumentation>();


        public string Name { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }


        internal NamespaceDocumentation(ModuleDocumentation moduleDocumentation, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null", nameof(name));

            m_ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            Name = name;
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);

            //TODO: Namespace XML docs
        }


        public IDocumentation TryGetDocumentation(MemberId member) => m_ModuleDocumentation.TryGetDocumentation(member);


        internal void AddType(TypeDocumentation typeDocumentation) => m_Types.Add(typeDocumentation.TypeId, typeDocumentation);
    }
}
