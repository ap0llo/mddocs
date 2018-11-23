using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    public abstract class MemberDocumentation: IDocumentation
    {
        public TypeDocumentation TypeDocumentation { get; }


        public MemberDocumentation(TypeDocumentation typeDocumentation)
        {
            TypeDocumentation = typeDocumentation ?? throw new ArgumentNullException(nameof(typeDocumentation));
        }

        public TypeDocumentation TryGetDocumentation(TypeReference typeReference) => 
            TypeDocumentation.TryGetDocumentation(typeReference);
    }
}
