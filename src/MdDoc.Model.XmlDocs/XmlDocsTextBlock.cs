using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public class XmlDocsTextBlock
    {
        private readonly XElement m_Xml;

        
        public IReadOnlyList<Element> Elements { get; }



        public XmlDocsTextBlock(XElement xml)
        {
            m_Xml = xml ?? throw new ArgumentNullException(nameof(xml));

            Elements = LoadElements().ToArray();          
            
        }


        private IEnumerable<Element> LoadElements()
        {
            string prefix = default;

            foreach(var node in m_Xml.Nodes())
            {
                switch (node)
                {
                    case XText textNode:
                        var text = textNode.Value;

                        if(node.PreviousNode == null)
                        {
                            text = text.TrimEmptyLines();
                            prefix = GetTrailingWhitespace(text);
                        }

                        if (prefix != null)
                            text = RemoveLinePrefix(text, prefix);

                        yield return new PlainTextElement(text);
                        break;

                    case XElement element:

                        switch(element.Name.LocalName)
                        {
                            case "see":
                                yield return new SeeElement(element.Attribute("cref").Value);
                                break;
                        }

                        break;

                    default:
                        break;
                }
            }
        }



        private string GetTrailingWhitespace(string text)
        {            
            var lines = text.Split("\r\n".ToCharArray());

            if (lines.Length == 0)
            {
                return null;
            }
            else
            {
                // detect whitespace prefix
                int i;
                for (i = 0; i < lines[0].Length && char.IsWhiteSpace(lines[0][i]); i++) ;
                var prefix = lines[0].Substring(0, i);
                return prefix;
            }
        }

        private string RemoveLinePrefix(string text, string prefix)
        {
            
            var lines = text.Split("\r\n".ToCharArray());

            if (lines.Length == 0)
            {
                return "";
            }
            else
            {                
                // remove prefix from all lines
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j].StartsWith(prefix))
                    {
                        lines[j] = lines[j].Remove(0, prefix.Length);
                    }
                }

                return String.Join(Environment.NewLine, lines);
            }
        }
        
    }
}
