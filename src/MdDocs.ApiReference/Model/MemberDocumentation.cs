using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class MemberDocumentation : IDocumentation
    {
        public TypeDocumentation TypeDocumentation { get; }


        public MemberDocumentation(TypeDocumentation typeDocumentation)
        {
            TypeDocumentation = typeDocumentation ?? throw new ArgumentNullException(nameof(typeDocumentation));
        }


        public abstract IDocumentation TryGetDocumentation(MemberId id);
    }
}
