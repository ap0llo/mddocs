using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class FieldDocumentation
    {
        private readonly DocumentationContext m_Context;
        
        public FieldDefinition Definition { get; }


        public FieldDocumentation(DocumentationContext context, FieldDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }
        
    }
}
