using System.Collections.Generic;

namespace MdDoc.Model
{
    public abstract class OverloadableMemberDocumentation<TOverload> : MemberDocumentation where TOverload : OverloadDocumentation
    {
        public abstract IReadOnlyCollection<TOverload> Overloads { get; }

        public OverloadableMemberDocumentation(TypeDocumentation typeDocumentation) : base(typeDocumentation)
        {
        }
    }
}
