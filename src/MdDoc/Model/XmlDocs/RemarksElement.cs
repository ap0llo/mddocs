using System.Collections.Generic;

namespace MdDoc.Model.XmlDocs
{
    public sealed class RemarksElement : ContainerElement
    {
        public RemarksElement(IEnumerable<Element> elements) : base(elements)
        { }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
