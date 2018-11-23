using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class ConstructorDocumentation : MethodDocumentation
    {
        public ConstructorDocumentation(TypeDocumentation typeDocumentation, DocumentationContext context, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation, context, definitions)
        {
        }
    }
}
