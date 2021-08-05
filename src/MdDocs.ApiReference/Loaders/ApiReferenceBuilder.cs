using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    internal sealed class ApiReferenceBuilder
    {
        private readonly Dictionary<string, _AssemblyDocumentation> m_Assemblies = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<NamespaceId, _NamespaceDocumentation> m_Namespaces = new();
        private readonly Dictionary<TypeId, _TypeDocumentation> m_Types = new();


        public IReadOnlyCollection<_AssemblyDocumentation> Assemblies { get; }

        public IReadOnlyCollection<_NamespaceDocumentation> Namespaces { get; }

        public _NamespaceDocumentation GlobalNamespace { get; } = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);

        public IReadOnlyCollection<_TypeDocumentation> Types { get; }


        public ApiReferenceBuilder()
        {
            Assemblies = ReadOnlyCollectionAdapter.Create(m_Assemblies.Values);
            Namespaces = ReadOnlyCollectionAdapter.Create(m_Namespaces.Values);
            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public _AssemblyDocumentation AddAssembly(string assemblyName, string? assemblyVersion)
        {
            if (String.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentException("Value must not be null or whitespace", nameof(assemblyName));

            if (m_Assemblies.ContainsKey(assemblyName))
            {
                throw new DuplicateItemException($"Assembly '{assemblyName}' already exists");
            }

            var assembly = new _AssemblyDocumentation(assemblyName, assemblyVersion);
            m_Assemblies.Add(assemblyName, assembly);

            return assembly;
        }

        public _AssemblyDocumentation GetAssembly(string assemblyName)
        {
            if (String.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentException("Value must not be null or whitespace", nameof(assemblyName));

            if (m_Assemblies.TryGetValue(assemblyName, out var existingAssembly))
            {
                return existingAssembly;
            }
            else
            {
                throw new ItemNotFoundException($"Assembly '{assemblyName}' was not found");
            }
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

        public _TypeDocumentation AddType(string assemblyName, TypeId typeId)
        {
            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (m_Types.ContainsKey(typeId))
            {
                throw new DuplicateItemException($"Type '{typeId}' already exists");
            }

            var assembly = GetAssembly(assemblyName);

            if (typeId.IsNestedType)
            {
                _TypeDocumentation declaringType;
                if (!m_Types.TryGetValue(typeId.DeclaringType!, out declaringType!))
                {
                    declaringType = AddType(assemblyName, typeId.DeclaringType!);
                }

                var nestedType = new _TypeDocumentation(assembly, declaringType, typeId);
                m_Types.Add(typeId, nestedType);
                declaringType.AddNestedType(nestedType);

                assembly.Add(nestedType);
                nestedType.Namespace.Add(nestedType);

                return nestedType;
            }
            else
            {

                var @namespace = GetOrAddNamespace(typeId.Namespace.Name);

                var type = new _TypeDocumentation(assembly, @namespace, typeId);
                m_Types.Add(typeId, type);

                assembly.Add(type);
                @namespace.Add(type);

                return type;
            }
        }

        public _TypeDocumentation GetType(TypeId typeId)
        {
            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (m_Types.TryGetValue(typeId, out var existingType))
            {
                return existingType;
            }
            else
            {
                throw new ItemNotFoundException($"Type '{typeId}' was not found");
            }
        }

        public _AssemblySetDocumentation Build()
        {
            return new _AssemblySetDocumentation(
                m_Assemblies.Values,
                m_Namespaces.Values,
                m_Types.Values
            );
        }
    }

}
