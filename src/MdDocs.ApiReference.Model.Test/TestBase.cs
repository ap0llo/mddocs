using System;
using System.Linq;
using System.Reflection;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Test
{
    public class TestBase : IDisposable
    {
        protected Lazy<AssemblyDefinition> m_AssemblyDefinition;
        protected Lazy<AssemblyDocumentation> m_AssemblyDocumentation;

        public TestBase()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(TestClass_Type)).Location;

            m_AssemblyDefinition = new Lazy<AssemblyDefinition>(() => AssemblyDefinition.ReadAssembly(assemblyPath));
            m_AssemblyDocumentation = new Lazy<AssemblyDocumentation>(
                () => new AssemblyDocumentation(m_AssemblyDefinition.Value, NullXmlDocsProvider.Instance, NullLogger.Instance)
            );
        }

        public void Dispose()
        {
            // disposing m_AssemblyDocumentation will implicitly dispose m_AssemblyDefinition

            if (m_AssemblyDocumentation.IsValueCreated)
                m_AssemblyDocumentation.Value.Dispose();
            else if (m_AssemblyDefinition.IsValueCreated)
                m_AssemblyDefinition.Value.Dispose();
        }


        protected TypeId GetTypeId(Type t) => GetTypeDefinition(t).ToTypeId();

        protected TypeDefinition GetTypeDefinition(Type t)
        {
            if(t.IsGenericType)
            {
                return GetTypeDefinition(t.Name, t.GetGenericArguments().Length);
            }

            return GetTypeDefinition(t.Name, 0);
        }

        protected TypeDefinition GetTypeDefinition(string name, int arity) => m_AssemblyDefinition.Value.MainModule.GetTypes().Single(typeDef => typeDef.Name == name && typeDef.GenericParameters.Count == arity);

        protected TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);

            var sut = new TypeDocumentation(
                m_AssemblyDocumentation.Value.MainModuleDocumentation,
                new NamespaceDocumentation(m_AssemblyDocumentation.Value.MainModuleDocumentation, null, new NamespaceId(type.Namespace), NullLogger.Instance),
                typeDefinition,
                NullXmlDocsProvider.Instance,
                NullLogger.Instance
            );

            return sut;
        }

        protected TypeDocumentation GetTypeDocumentation(string typeName, int arity)
        {
            var typeDefinition = GetTypeDefinition(typeName, arity);

            var sut = new TypeDocumentation(
                m_AssemblyDocumentation.Value.MainModuleDocumentation,
                new NamespaceDocumentation(m_AssemblyDocumentation.Value.MainModuleDocumentation, null, new NamespaceId(typeDefinition.Namespace), NullLogger.Instance),
                typeDefinition,
                NullXmlDocsProvider.Instance,
                NullLogger.Instance
            );

            return sut;
        }
    }
}
