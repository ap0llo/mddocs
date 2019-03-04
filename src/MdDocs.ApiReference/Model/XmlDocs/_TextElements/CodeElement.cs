using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class CodeElement : Element
    {
        public string Content { get; }


        public CodeElement(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

     
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
