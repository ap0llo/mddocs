using NuDoq;
using System;

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

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
