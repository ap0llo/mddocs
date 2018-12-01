using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public class XmlDocsTextBlock
    {
        public string Text { get; }

        public XmlDocsTextBlock(XElement textNode)
        {
            var text = textNode.Value.TrimEmptyLines();
            
            var lines = text.Split("\r\n".ToCharArray());
                    
            if(lines.Length == 0)
            {
                Text = "";
            }
            else
            {
                // detect whitespace prefix
                int i;
                for (i = 0; i < lines[0].Length && char.IsWhiteSpace(lines[0][i]); i++) ;
                var prefix = lines[0].Substring(0, i);

                // remove prefix from all lines
                for (int j = 0; j < lines.Length; j++)
                {
                    if(lines[j].StartsWith(prefix))
                    {
                        lines[j] = lines[j].Remove(0, prefix.Length);
                    }
                }

                Text = String.Join(Environment.NewLine, lines).TrimEnd('\r', '\n');
            }



        }

    }
}
