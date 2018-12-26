using System;
using System.Linq;
using System.Reflection;
using MdDoc.Model;
using MdDoc.Model.XmlDocs;
using MdDoc.Test.TestData;
using Mono.Cecil;

namespace MdDoc.Test
{
    public class TestBase : IDisposable
    {
        protected Lazy<AssemblyDefinition> m_AssemblyDefinition;
        protected Lazy<AssemblyDocumentation> m_AssemblyDocumentation;

        public TestBase()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(TestClass_Type)).Location;

            m_AssemblyDefinition = new Lazy<AssemblyDefinition>(() => AssemblyDefinition.ReadAssembly(assemblyPath));
            m_AssemblyDocumentation = new Lazy<AssemblyDocumentation>(() => new AssemblyDocumentation(m_AssemblyDefinition.Value, NullXmlDocsProvider.Instance));
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

        protected TypeDefinition GetTypeDefinition(Type t) => GetTypeDefinition(t.Name);

        protected TypeDefinition GetTypeDefinition(string name) => m_AssemblyDefinition.Value.MainModule.GetTypes().Single(typeDef => typeDef.Name == name);

        protected TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);

            var sut = new TypeDocumentation(
                m_AssemblyDocumentation.Value.MainModuleDocumentation,
                typeDefinition,
                NullXmlDocsProvider.Instance
            );

            return sut;
        }

        protected TypeDocumentation GetTypeDocumentation(string typeName)
        {
            var typeDefinition = GetTypeDefinition(typeName);

            var sut = new TypeDocumentation(
                m_AssemblyDocumentation.Value.MainModuleDocumentation,
                typeDefinition,
                NullXmlDocsProvider.Instance
            );

            return sut;
        }

    }
}
