using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class PropertyDocumentation
    {
        private readonly DocumentationContext m_Context;
        
        public PropertyDefinition Definition { get; }


        public PropertyDocumentation(DocumentationContext context, PropertyDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        
    }
}
