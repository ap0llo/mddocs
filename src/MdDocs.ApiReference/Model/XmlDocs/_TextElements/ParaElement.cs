using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public class ParaElement : Element
    {
        public TextBlock Text { get; }

        public ParaElement(TextBlock text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
