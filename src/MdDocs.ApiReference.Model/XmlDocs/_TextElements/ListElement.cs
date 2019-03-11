using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class ListElement : Element
    {
        public ListType Type { get; }

        public ListItemElement ListHeader { get; }

        public IReadOnlyList<ListItemElement> Items { get; }


        public ListElement(ListType type, ListItemElement listHeader, IReadOnlyList<ListItemElement> items)
        {
            Type = type;
            ListHeader = listHeader;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }


        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
