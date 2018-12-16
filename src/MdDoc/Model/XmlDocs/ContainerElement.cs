using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public abstract class ContainerElement : Element
    {       

        public IReadOnlyList<Element> Elements { get; }


        internal ContainerElement(IEnumerable<Element> elements)
        {
            Elements = elements?.ToArray() ?? throw new ArgumentNullException(nameof(elements));
        }
    }
}
