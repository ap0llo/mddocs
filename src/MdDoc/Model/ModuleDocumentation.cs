using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class ModuleDocumentation
    {
        private readonly ModuleDefinition m_Module;
        private readonly DocumentationContext m_Context;

        public IReadOnlyCollection<TypeDocumentation> Types { get; }



        public ModuleDocumentation(ModuleDefinition module)
        {
            m_Module = module ?? throw new ArgumentNullException(nameof(module));
            m_Context = new DocumentationContext(module, NullXmlDocProvider.Instance);

            Types = m_Module
                .Types
                .Where(m_Context.IsDocumentedItem)
                .Select(typeDefinition => new TypeDocumentation(m_Context, typeDefinition))
                .ToArray();
        }        
    }
}
