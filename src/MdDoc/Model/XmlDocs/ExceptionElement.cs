using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ExceptionElement : ContainerElement
    {
        private readonly NuDoq.Exception m_NuDoqModel;

        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }


        public ExceptionElement(NuDoq.Exception nuDoqModel, IEnumerable<Element> elements) : base(elements)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
