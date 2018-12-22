using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class TextBlock : Element
    {       
        public IReadOnlyList<Element> Elements { get; }


        internal TextBlock(IEnumerable<Element> elements)
        {
            Elements = elements?.ToArray() ?? throw new ArgumentNullException(nameof(elements));
        }

        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
