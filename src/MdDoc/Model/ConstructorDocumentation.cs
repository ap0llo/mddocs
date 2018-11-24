using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class ConstructorDocumentation : MethodDocumentation
    {
        public ConstructorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation, definitions)
        {
        }
    }
}
