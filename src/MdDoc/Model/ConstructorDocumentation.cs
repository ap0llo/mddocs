using Mono.Cecil;
using System.Collections.Generic;

namespace MdDoc.Model
{
    public class ConstructorDocumentation : MethodDocumentation
    {
        public ConstructorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions) 
            : base(typeDocumentation, definitions)
        {
        }
    }
}
