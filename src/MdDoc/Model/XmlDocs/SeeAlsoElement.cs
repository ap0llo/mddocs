using NuDoq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public class SeeAlsoElement : Element
    {
        private readonly SeeAlso m_NuDoqModel;

        public string Cref => m_NuDoqModel.Cref;


        public SeeAlsoElement(SeeAlso nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);

    }
}
