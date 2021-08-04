using System;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public sealed class AssemblySetDocumentationBuilder
    {
        private readonly AssemblyDocumentationBuilder m_AssemblyBuilder = new();
        private readonly NamespaceDocumentationBuilder m_NamespaceBuilder = new();
        private readonly TypeDocumentationBuilder m_TypeBuilder = new();

        public _AssemblyDocumentation AddAssembly(string assemblyName, string? assemblyVersion) =>
                m_AssemblyBuilder.AddAssembly(assemblyName, assemblyVersion);

        public _AssemblyDocumentation GetOrAddAssembly(string assemblyName, string? assemblyVersion) =>
            m_AssemblyBuilder.GetOrAddAssembly(assemblyName, assemblyVersion);

        public _NamespaceDocumentation GetOrAddNamespace(string namespaceName) =>
            m_NamespaceBuilder.GetOrAddNamespace(namespaceName);

        public _NamespaceDocumentation AddNamespace(string namespaceName) =>
            m_NamespaceBuilder.AddNamespace(namespaceName);

        public _TypeDocumentation AddType(string assemblyName, TypeId typeId)
        {
            var assembly = GetOrAddAssembly(assemblyName, null);
            var @namespace = m_NamespaceBuilder.GetOrAddNamespace(typeId.Namespace);

            var type = m_TypeBuilder.AddType(assembly, @namespace, typeId);

            assembly.Add(type);
            @namespace.Add(type);

            return type;
        }


        public _AssemblySetDocumentation Build()
        {
            return new _AssemblySetDocumentation(
                m_AssemblyBuilder.Assemblies,
                m_NamespaceBuilder.Namespaces,
                m_TypeBuilder.Types
            );
        }
    }

}
