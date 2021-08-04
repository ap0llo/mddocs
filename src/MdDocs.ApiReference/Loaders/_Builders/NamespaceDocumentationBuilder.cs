using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public sealed class NamespaceDocumentationBuilder
    {
        private readonly Dictionary<NamespaceId, _NamespaceDocumentation> m_Namespaces = new();


        public IReadOnlyCollection<_NamespaceDocumentation> Namespaces { get; }

        public _NamespaceDocumentation GlobalNamespace { get; } = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);


        public NamespaceDocumentationBuilder()
        {
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
        }


        public _NamespaceDocumentation GetOrAddNamespace(string namespaceName)
        {
            if (String.IsNullOrWhiteSpace(namespaceName) && namespaceName != "")
                throw new ArgumentException("Value must not be null or whitespace", nameof(namespaceName));

            if (m_Namespaces.Count == 0)
            {
                m_Namespaces.Add(NamespaceId.GlobalNamespace, GlobalNamespace);
            }


            var namespaceId = new NamespaceId(namespaceName);

            if (m_Namespaces.TryGetValue(namespaceId, out var existingNamespace))
            {
                return existingNamespace;
            }

            var names = namespaceName.Split('.');
            var parentNamespace = names.Length > 1
                ? GetOrAddNamespace(names.Take(names.Length - 1).JoinToString("."))
                : GlobalNamespace;


            var @namespace = new _NamespaceDocumentation(parentNamespace, namespaceId);
            m_Namespaces.Add(namespaceId, @namespace);
            parentNamespace.Add(@namespace);


            return @namespace;

            //if (namespaces.ContainsKey(namespaceId))
            //{
            //    return namespaces[namespaceId];
            //}



            //var parentNamespace = names.Length > 1
            //    ? GetOrAddNamespace(namespaces, names.Take(names.Length - 1).JoinToString("."))
            //    : _NamespaceDocumentation.GlobalNamespace;

            //var newNamespace = new _NamespaceDocumentation(parentNamespace, namespaceId);
            //namespaces.Add(namespaceId, newNamespace);

            //parentNamespace.AddNamespace(newNamespace);

            //return newNamespace;
        }

    }
}
