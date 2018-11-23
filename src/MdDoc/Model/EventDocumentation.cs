using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class EventDocumentation : MemberDocumentation
    {
        private readonly DocumentationContext m_Context;
        
        public EventDefinition Definition { get; }


        public EventDocumentation(TypeDocumentation typeDocumentation, DocumentationContext context, EventDefinition definition) : base(typeDocumentation)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }
        
    }
}
