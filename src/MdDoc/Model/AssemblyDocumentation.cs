using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class AssemblyDocumentation : IDisposable
    {        
        private readonly AssemblyDefinition m_Assembly;

        public ModuleDocumentation MainModule { get; }

        public DocumentationContext Context { get; }


        private AssemblyDocumentation(DocumentationContext context, AssemblyDefinition assembly)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            m_Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            MainModule = new ModuleDocumentation(Context, assembly.MainModule);
        }


        public void Dispose()
        {
            m_Assembly.Dispose();
        }


        public static AssemblyDocumentation FromFile(string filePath)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);
            //TODO: Load real xml docs
            var context = new DocumentationContext(assemblyDefinition.MainModule, NullXmlDocProvider.Instance);

            return new AssemblyDocumentation(context, assemblyDefinition);
        }
    }
}
