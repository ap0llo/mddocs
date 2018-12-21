using MdDoc.Model;
using MdDoc.Test.TestData;
using MdDoc.Model.XmlDocs;
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

        
        protected TypeId GetTypeId(Type t) => GetTypeDefinition(t).ToTypeId();

        protected TypeDefinition GetTypeDefinition(Type t)
        {
            return GetTypeDefinition(t.Name);
        }

        protected TypeDefinition GetTypeDefinition(string  name)
        {
            return m_AssemblyDocumentation.MainModuleDocumentation.Definition.GetTypes().Single(typeDef => typeDef.Name == name);
        }


        protected TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);
            var sut = new TypeDocumentation(m_AssemblyDocumentation.MainModuleDocumentation, typeDefinition, new NullXmlDocsProvider());
            return sut;
        }

    }
}
