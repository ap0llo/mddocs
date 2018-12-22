using System;

namespace MdDoc.Model.XmlDocs
{
    public class ParaElement : Element
    {
        public TextBlock Text { get; }

        public ParaElement(TextBlock text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
