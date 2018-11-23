using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class ModuleDocumentation
    {
        private readonly DocumentationContext m_Context;


        public AssemblyDocumentation AssemblyDocumentation { get; }

        public ModuleDefinition Definition { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }



        public ModuleDocumentation(AssemblyDocumentation assemblyDocumentation, DocumentationContext context, ModuleDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            AssemblyDocumentation = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));

            Types = Definition
                .Types
                .Where(m_Context.IsDocumentedItem)
                .Select(typeDefinition => new TypeDocumentation(this, m_Context, typeDefinition))
                .ToArray();
        }        
    }
}
