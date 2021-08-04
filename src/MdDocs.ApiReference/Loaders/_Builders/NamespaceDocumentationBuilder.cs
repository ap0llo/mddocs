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


        public _NamespaceDocumentation AddNamespace(string namespaceName)
        {
            if (String.IsNullOrWhiteSpace(namespaceName) && namespaceName != "")
                throw new ArgumentException("Value must not be null or whitespace", nameof(namespaceName));

            var namespaceId = new NamespaceId(namespaceName);

            if (m_Namespaces.ContainsKey(namespaceId))
            {
                throw new DuplicateItemException($"Namespace '{namespaceName}' already exists");
            }

            return GetOrAddNamespace(namespaceName);
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

        }

        //TODO 2021-08-04: Add tests
        public _NamespaceDocumentation GetOrAddNamespace(NamespaceId namespaceId)
        {
            if (namespaceId is null)
                throw new ArgumentNullException(nameof(namespaceId));

            if (m_Namespaces.Count == 0)
            {
                m_Namespaces.Add(NamespaceId.GlobalNamespace, GlobalNamespace);
            }

            if (m_Namespaces.TryGetValue(namespaceId, out var existingNamespace))
            {
                return existingNamespace;
            }

            var names = namespaceId.Name.Split('.');
            var parentNamespace = names.Length > 1
                ? GetOrAddNamespace(names.Take(names.Length - 1).JoinToString("."))
                : GlobalNamespace;

            var @namespace = new _NamespaceDocumentation(parentNamespace, namespaceId);
            m_Namespaces.Add(namespaceId, @namespace);
            parentNamespace.Add(@namespace);

            return @namespace;

        }


    }
}
