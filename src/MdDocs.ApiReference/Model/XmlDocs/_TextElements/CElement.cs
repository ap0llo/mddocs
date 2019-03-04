using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class CElement : Element
    {
        public string Content { get; }


        public CElement(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
