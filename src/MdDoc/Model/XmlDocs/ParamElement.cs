using System;
using System.Collections.Generic;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ParamElement : ContainerElement
    {
        private readonly Param m_NuDoqModel;

        public string Name => m_NuDoqModel.Name;


        public ParamElement(Param nuDoqModel, IEnumerable<Element> elements) : base(elements)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
