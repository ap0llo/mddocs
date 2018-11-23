using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class FieldDocumentation : MemberDocumentation
    {
        private readonly DocumentationContext m_Context;
        
        public FieldDefinition Definition { get; }


        public FieldDocumentation(TypeDocumentation typeDocumentation, DocumentationContext context, FieldDefinition definition) : base(typeDocumentation)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }
        
    }
}
