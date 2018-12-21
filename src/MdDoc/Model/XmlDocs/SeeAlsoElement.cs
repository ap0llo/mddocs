using System;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public class SeeAlsoElement : Element
    {
        private readonly SeeAlso m_NuDoqModel;

        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }


        public SeeAlsoElement(SeeAlso nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }


        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
