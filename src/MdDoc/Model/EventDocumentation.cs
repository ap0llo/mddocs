using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class EventDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public MemberId MemberId { get; }

        internal EventDefinition Definition { get; }


        public EventDocumentation(TypeDocumentation typeDocumentation, EventDefinition definition) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();
        }
        
    }
}
