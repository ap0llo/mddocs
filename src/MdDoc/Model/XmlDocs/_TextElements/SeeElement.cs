using System;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class SeeElement : Element
    {
        private readonly See m_NuDoqModel;


        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }


        public SeeElement(See nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
