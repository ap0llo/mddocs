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



        private AssemblyDocumentation(AssemblyDefinition assembly)
        {
            Definition = assembly ?? throw new ArgumentNullException(nameof(assembly));

            MainModuleDocumentation = new ModuleDocumentation(this, assembly.MainModule);
        }


        public void Dispose()
        {
            Definition.Dispose();
        }


        public static AssemblyDocumentation FromFile(string filePath)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);
                        
            return new AssemblyDocumentation(assemblyDefinition);
        }

        public TypeDocumentation TryGetDocumentation(TypeReference typeReference) => 
            MainModuleDocumentation.TryGetDocumentation(typeReference);
    }
}
