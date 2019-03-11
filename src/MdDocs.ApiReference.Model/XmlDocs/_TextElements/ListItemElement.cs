using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class ListItemElement : Element
    {
        public TextBlock Term { get; }

        public TextBlock Description { get; }


        public ListItemElement(TextBlock term, TextBlock description)
        {            
            Term = term;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
