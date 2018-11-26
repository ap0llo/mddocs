using System.Collections.Generic;

namespace MdDoc.Model
{
    public class ConstructorDocumentation : MethodDocumentation
    {
        public ConstructorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodOverload> overloads) 
            : base(typeDocumentation, overloads)
        {
        }
    }
}
