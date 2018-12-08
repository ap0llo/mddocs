using MdDoc.Model;
using MdDoc.Test.TestData;
using MdDoc.XmlDocs;
using Mono.Cecil;
using System;
using System.Linq;
using System.Reflection;

namespace MdDoc.Test
{
    public class TestBase : IDisposable
    {
        protected AssemblyDocumentation m_AssemblyDocumentation;

        public TestBase()
        {
            m_AssemblyDocumentation = AssemblyDocumentation.FromFile(Assembly.GetAssembly(typeof(TestClass_Type)).Location);                        
        }

        public void Dispose()
        {
            m_AssemblyDocumentation.Dispose();
        }


        protected TypeName GetTypeName(Type t) => GetTypeDefinition(t).ToTypeName();
        
        protected TypeDefinition GetTypeDefinition(Type t)
        {
            return m_AssemblyDocumentation.MainModuleDocumentation.Definition.GetTypes().Single(typeDef => typeDef.Name == t.Name);
        }

        protected TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);
            var sut = new TypeDocumentation(m_AssemblyDocumentation.MainModuleDocumentation, typeDefinition, new NullXmlDocsProvider());
            return sut;
        }

    }
}
