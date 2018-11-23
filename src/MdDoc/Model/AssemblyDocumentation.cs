using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class AssemblyDocumentation : IDisposable, IDocumentation
    {
        public AssemblyDefinition Definition { get; }

        public ModuleDocumentation MainModuleDocumentation { get; }

        public DocumentationContext Context { get; }


        private AssemblyDocumentation(DocumentationContext context, AssemblyDefinition assembly)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = assembly ?? throw new ArgumentNullException(nameof(assembly));

            MainModuleDocumentation = new ModuleDocumentation(this, Context, assembly.MainModule);
        }


        public void Dispose()
        {
            Definition.Dispose();
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
