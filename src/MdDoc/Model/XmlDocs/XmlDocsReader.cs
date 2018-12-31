// The code in this file is dervide from the "DocReader" class from the NuDoq project.
// The original version of this file was downloaded from
// https://github.com/kzu/NuDoq/blob/56ad8c508003490d859214753591440b123616f5/src/NuDoq/DocReader.cs
//
// See the original license below for details
//
#region Apache Licensed
/*
 Copyright 2013 Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace MdDoc.Model.XmlDocs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;    
    using System.Xml;
    using System.Xml.Linq;
    
    /// <summary>
    /// Reads .NET XML API documentation files.
    /// </summary>
    public static class XmlDocsReader
    {
        /// <summary>
        /// Reads the specified documentation file and returns list of members.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static IReadOnlyList<MemberElement> Read(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => ReadMember(element.Attribute("name").Value, element))
                .ToList();
        }

        
        /// <summary>
        /// Reads all documentation for a single member
        /// </summary>
        private static MemberElement ReadMember(string memberId, XElement element)
        {
            var id = MemberId.Parse(memberId);
            var memberElement = new MemberElement(id);

            ReadMemberContent(element, memberElement);

            return memberElement;
        }


        /// <summary>
        /// Reads all supported documentation elements and adds it to the specified member.
        /// </summary>
        private static void ReadMemberContent(XElement xml, MemberElement member)
        {
            foreach (var elementNode in xml.Elements())
            {                
                switch (elementNode.Name.LocalName)
                {
                    case "summary":
                        member.Summary = ReadTextBlock(elementNode);
                        break;
                    case "remarks":
                        member.Remarks = ReadTextBlock(elementNode);
                        break;
                    case "example":
                        member.Example = ReadTextBlock(elementNode);
                        break;
                    case "param":
                        {
                            var name = FindAttribute(elementNode, "name");
                            if(name != null)
                                member.Parameters.Add(name, ReadTextBlock(elementNode));
                        }
                        break;
                    case "typeparam":
                        {
                            var name = FindAttribute(elementNode, "name");
                            if(name != null)
                                member.TypeParameters.Add(name, ReadTextBlock(elementNode));
                        }
                        break;                    
                    case "seealso":
                        {
                            var cref = FindAttribute(elementNode, "cref");
                            if(cref != null)
                            {
                                var memberId = MemberId.Parse(cref);
                                member.SeeAlso.Add(new SeeAlsoElement(memberId, ReadTextBlock(elementNode)));
                            }
                        }
                        break;
                    case "exception":
                        {
                            var cref = FindAttribute(elementNode, "cref");
                            if(cref != null)
                            {
                                var memberId = MemberId.Parse(cref);
                                member.Exceptions.Add(new ExceptionElement(memberId, ReadTextBlock(elementNode)));
                            }
                        }
                        break;
                    case "value":
                        member.Value = ReadTextBlock(elementNode);
                        break;
                    case "returns":
                        member.Returns = ReadTextBlock(elementNode);
                        break;

                    // ignore unknown elements
                    default:
                        //TODO: Emit warning about unknown element
                        break;
                }                
            }
        }

        /// <summary>
        /// Reads all supported text elements.
        /// </summary>
        private static TextBlock ReadTextBlock(XElement xml)
        {
            var textElements = new List<Element>();

            foreach (var node in xml.Nodes())
            {
                var element = default(Element);
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        switch (elementNode.Name.LocalName)
                        {
                            case "para":
                                element = new ParaElement(ReadTextBlock(elementNode));
                                break;
                            case "paramref":
                                element = new ParamRefElement(FindAttribute(elementNode, "name"));
                                break;
                            case "typeparamref":
                                element = new TypeParamRefElement(FindAttribute(elementNode, "name"));
                                break;
                            case "code":
                                //TODO: Support lang attribute
                                element = new CodeElement(TrimCode(elementNode.Value));
                                break;
                            case "c":
                                element = new CElement(elementNode.Value);
                                break;
                            case "see":
                                
                                element = new SeeElement(new MemberIdParser(FindAttribute(elementNode, "cref")).Parse());
                                break;
                            //case "list":
                            //    element = new List(FindAttribute(elementNode, "type"), ReadContent(elementNode));
                            //    break;
                            //case "listheader":
                            //    element = new ListHeader(ReadContent(elementNode));
                            //    break;
                            //case "term":
                            //    element = new Term(ReadContent(elementNode));
                            //    break;
                            //case "description":
                            //    element = new Description(ReadContent(elementNode));
                            //    break;
                            //case "item":
                            //    element = new Item(ReadContent(elementNode));
                            //    break;

                            // ignore unknown elements
                            default:
                                //TODO: Emit warnign for unknown elements
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        element = new TextElement(TrimText(((XText)node).Value));
                        break;
                    default:
                        break;
                }

                if (element != null)
                {
                    textElements.Add(element);
                }
            }

            return new TextBlock(textElements);
        }



        /// <summary>
        /// Retrieves an attribute value if found, otherwise, returns a null string.
        /// </summary>
        private static string FindAttribute(XElement elementNode, string attributeName)
        {
            return elementNode.Attributes().Where(x => x.Name == attributeName).Select(x => x.Value).FirstOrDefault();
        }

        /// <summary>
        /// Trims the text by removing new lines and trimming the indent.
        /// </summary>
        private static string TrimText(string content)
        {
            return TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ");
        }

        /// <summary>
        /// Trims the code by removing extra indent.
        /// </summary>
        private static string TrimCode(string content)
        {
            return TrimLines(content, StringSplitOptions.None, Environment.NewLine);
        }

        private static string TrimLines(string content, StringSplitOptions splitOptions, string joinWith)
        {
            var lines = content.Split(new[] { Environment.NewLine, "\n" }, splitOptions).ToList();

            if (lines.Count == 0)
                return string.Empty;

            // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
            if (lines[0].Trim().Length == 0)
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return string.Empty;

            if (lines[lines.Count - 1].Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return string.Empty;

            // The indent of the first line of content determines the base 
            // indent for all the lines, which   we should remove since it's just 
            // a doc gen artifact.
            var indent = lines[0].TakeWhile(c => char.IsWhiteSpace(c)).Count();
            // Indent in generated XML doc files is greater than 4 always. 
            // This allows us to optimize the case where the author actually placed 
            // whitespace inline in between tags.
            if (indent <= 4 && !string.IsNullOrEmpty(lines[0]) && lines[0][0] != '\t')
                indent = 0;

            return string.Join(joinWith, lines
                .Select(line =>
                {
                    if (string.IsNullOrEmpty(line))
                        return line;
                    else if (line.Length < indent)
                        return string.Empty;
                    else
                        return line.Substring(indent);
                })
                .ToArray());
        }
    }
}
