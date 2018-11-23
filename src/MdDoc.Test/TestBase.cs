using MdDoc.Model;
using MdDoc.Test.TestData;
using Mono.Cecil;
using System;
using System.Linq;
using System.Reflection;

namespace MdDoc.Test
{
    public class TestBase : IDisposable
    {
        protected AssemblyDocumentation m_AssemblyDocumentation;
        protected readonly DocumentationContext m_Context;

        public TestBase()
        {
            m_AssemblyDocumentation = AssemblyDocumentation.FromFile(Assembly.GetAssembly(typeof(TestClass_Type)).Location);            
            m_Context = new DocumentationContext(m_AssemblyDocumentation.MainModuleDocumentation.Definition, NullXmlDocProvider.Instance);
        }

        public void Dispose()
        {
            m_AssemblyDocumentation.Dispose();
        }


        protected TypeDefinition GetTypeDefinition(Type t)
        {
            return m_AssemblyDocumentation.MainModuleDocumentation.Definition.GetTypes().Single(typeDef => typeDef.Name == t.Name);
        }

        protected TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);
            var sut = new TypeDocumentation(m_AssemblyDocumentation.MainModuleDocumentation, m_Context, typeDefinition);
            return sut;
        }

    }
}
