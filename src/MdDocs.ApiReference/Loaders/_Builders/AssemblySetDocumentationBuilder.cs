using System;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public sealed class AssemblySetDocumentationBuilder
    {
        private readonly AssemblyDocumentationBuilder m_AssembliesBuilder = new();
        private readonly NamespaceDocumentationBuilder m_NamespacesBuilder = new();


        public _AssemblyDocumentation GetOrAddAssembly(string assemblyName, string? assemblyVersion) =>
            m_AssembliesBuilder.GetOrAddAssembly(assemblyName, assemblyVersion);

        public _NamespaceDocumentation GetOrAddNamespace(string namespaceName) =>
            m_NamespacesBuilder.GetOrAddNamespace(namespaceName);

        public _AssemblySetDocumentation Build()
        {
            return new _AssemblySetDocumentation(m_AssembliesBuilder.Assemblies, m_NamespacesBuilder.Namespaces, Array.Empty<TypeDocumentation>());
        }
    }

}
