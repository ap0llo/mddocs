using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public abstract class MemberDocumentation
    {
        public TypeDocumentation TypeDocumentation { get; }


        public MemberDocumentation(TypeDocumentation typeDocumentation)
        {
            TypeDocumentation = typeDocumentation ?? throw new ArgumentNullException(nameof(typeDocumentation));
        }

    }
}
