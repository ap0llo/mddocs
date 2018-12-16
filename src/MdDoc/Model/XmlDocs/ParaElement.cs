using System;
using System.Collections.Generic;

namespace MdDoc.Model.XmlDocs
{
    public class ParaElement : ContainerElement
    {
        public ParaElement(IEnumerable<Element> elements) : base(elements)
        { }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
