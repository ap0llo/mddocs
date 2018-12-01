using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs
{
    public class XmlDocsTextBlock
    {
        public string Text { get; }

        public XmlDocsTextBlock(XElement textNode)
        {
            Text = textNode.Value.Trim();
        }

    }
}
