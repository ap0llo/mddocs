using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class SeeElement : Element
    {
        public MemberId MemberId { get; }


        public SeeElement(MemberId memberId)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
