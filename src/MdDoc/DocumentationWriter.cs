using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc
{
    public class DocumentationWriter : IDisposable
    {
        private readonly PathProvider m_PathProvider;
        private readonly string m_AssemblyFilePath;
        private readonly AssemblyDefinition m_Assembly;
        private readonly ModuleDefinition m_Module;
        private readonly DocumentationContext m_Context;

        public DocumentationWriter(string assemblyFilePath, string outDir)
        {            
            m_PathProvider = new PathProvider(outDir);
            m_AssemblyFilePath = assemblyFilePath;


            m_Assembly = AssemblyDefinition.ReadAssembly(assemblyFilePath);
            m_Module = m_Assembly.MainModule;
            m_Context = new DocumentationContext(m_Module);            
        }

        
        public void SaveDocumentation()
        {
            foreach(var type in m_Assembly.MainModule.Types.Where(m_Context.IsDocumentedItem))
            {
                Console.WriteLine($"Generating documentation for type {type.Namespace}.{type.Name}");

                var writer = new TypeDocumentationWriter(m_Context, m_PathProvider, type);
                writer.SaveDocumentation();
            }
        }


        public void Dispose()
        {
            m_Module.Dispose();
            m_Assembly.Dispose();
        }        
    }
}
