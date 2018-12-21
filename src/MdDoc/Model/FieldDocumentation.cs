using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class FieldDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public MemberId MemberId { get; }       

        internal FieldDefinition Definition { get; }


        public FieldDocumentation(TypeDocumentation typeDocumentation, FieldDefinition definition) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();
        }

        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
