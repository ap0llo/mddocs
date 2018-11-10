using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class EventDocumentation
    {
        private readonly DocumentationContext m_Context;
        
        public EventDefinition Definition { get; }


        public EventDocumentation(DocumentationContext context, EventDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }
        
    }
}
