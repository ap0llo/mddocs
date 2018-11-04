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
            foreach(var page in GetPages())
            {
                Console.WriteLine($"Saving page {page.Name}");
                page.Save();
            }            
        }


        public void Dispose()
        {
            m_Module.Dispose();
            m_Assembly.Dispose();
        }        

        private IEnumerable<IPage> GetPages()
        {
            foreach (var type in m_Assembly.MainModule.Types.Where(m_Context.IsDocumentedItem))
            {                
                yield return new TypePage(m_Context, m_PathProvider, type);                

                foreach (var property in type.Properties.Where(m_Context.IsDocumentedItem))
                {
                    yield return new PropertyPage(m_Context, m_PathProvider, property);                    
                }
            }
        }
    }
}
