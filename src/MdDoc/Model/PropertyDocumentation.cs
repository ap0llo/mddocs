using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class PropertyDocumentation : MemberDocumentation
    {        
        public PropertyDefinition Definition { get; }


        public PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        
    }
}
