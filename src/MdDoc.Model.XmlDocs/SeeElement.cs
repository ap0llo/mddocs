using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public sealed class SeeElement : Element
    {
        public SeeElement(string cref)
        {
            Cref = cref;
        }

        public string Cref { get; }
    }
}
