using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class TypeParamRefElement : Element
    {
        public string Name { get; }


        public TypeParamRefElement(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
