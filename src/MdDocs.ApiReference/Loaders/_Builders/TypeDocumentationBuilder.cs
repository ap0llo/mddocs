using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public sealed class TypeDocumentationBuilder
    {
        private readonly Dictionary<TypeId, _TypeDocumentation> m_Types = new();


        public IReadOnlyCollection<_TypeDocumentation> Types { get; }


        public TypeDocumentationBuilder()
        {
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public _TypeDocumentation AddType(_AssemblyDocumentation assembly, _NamespaceDocumentation @namespace, TypeId typeId)
        {
            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (m_Types.ContainsKey(typeId))
            {
                throw new DuplicateItemException($"Type '{typeId}' already exists");
            }

            var type = new _TypeDocumentation(assembly, @namespace, typeId);
            m_Types.Add(typeId, type);
            return type;

        }

        public _TypeDocumentation GetOrAddType(_AssemblyDocumentation assembly, _NamespaceDocumentation @namespace, TypeId typeId)
        {
            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (m_Types.TryGetValue(typeId, out var existingType))
            {
                return existingType;
            }

            var type = new _TypeDocumentation(assembly, @namespace, typeId);
            m_Types.Add(typeId, type);
            return type;

        }

    }
}
