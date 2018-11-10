using MdDoc.Test.TestData;
using Mono.Cecil;
using System;
using System.Linq;
using System.Reflection;

namespace MdDoc.Test
{
    public class TestBase : IDisposable
    {
        protected readonly AssemblyDefinition m_Assembly;
        protected readonly ModuleDefinition m_Module;

        public TestBase()
        {
            m_Assembly = AssemblyDefinition.ReadAssembly(Assembly.GetAssembly(typeof(TestClass_Type)).Location);
            m_Module = m_Assembly.MainModule;
        }

        public void Dispose()
        {
            m_Module.Dispose();
            m_Assembly.Dispose();
        }


        protected TypeDefinition GetTypeDefinition(Type t)
        {
            return m_Module.GetTypes().Single(typeDef => typeDef.Name == t.Name);
        }

    }
}
