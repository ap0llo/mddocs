using System;

namespace MdDoc.Model.XmlDocs
{
    public class TextElement : Element
    {
        public string Content { get; }


        public TextElement(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
