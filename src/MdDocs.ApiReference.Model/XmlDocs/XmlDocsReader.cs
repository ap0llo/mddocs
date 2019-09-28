// The code in this file is derived from the "DocReader" class from the NuDoq project.
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Reads .NET XML API documentation files.
    /// </summary>
    /// <remarks>
    /// This implementation is largely based on the <c>DocReader</c> class from the NuDoq project,
    /// Copyright 2013 Daniel Cazzulino, licensed under the Apache License.
    /// <para>
    /// The original version of this file is available at
    /// https://github.com/kzu/NuDoq/blob/56ad8c508003490d859214753591440b123616f5/src/NuDoq/DocReader.cs
    /// </para>
    /// </remarks>
    internal class XmlDocsReader
    {
        private readonly ILogger m_Logger;


        public XmlDocsReader(ILogger logger)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Reads the specified documentation file and returns list of members.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the specified parameters is <c>null</c></exception>
        public IReadOnlyList<MemberElement> Read(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            m_Logger.LogInformation($"Reading XML documentation comments from '{fileName}'");

            var document = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return Read(document);
        }

        /// <summary>
        /// Reads the specified documentation document and returns list of members.
        /// </summary>
        /// <param name="document">The XML documentation file to read.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the specified parameters is <c>null</c></exception>
        public IReadOnlyList<MemberElement> Read(XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            return document.Root.Element("members")
                .Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => TryReadMember(element))
                .Where(x => x != null)
                .ToList();
        }


        /// <summary>
        /// Reads all documentation for a single member
        /// </summary>
        private MemberElement TryReadMember(XElement element)
        {
            var name = element.Attribute("name")?.Value;

            if (!TryParseMemberId(name, out var id))
            {
                return null;
            }

            var memberElement = new MemberElement(id);

            ReadMemberContent(element, memberElement);

            return memberElement;
        }


        /// <summary>
        /// Reads all supported documentation elements and adds it to the specified member.
        /// </summary>
        private void ReadMemberContent(XElement xml, MemberElement member)
        {
            foreach (var element in xml.Elements())
            {
                var elementName = element.Name.LocalName;

                m_Logger.LogDebug($"Reading XML documentation element '{elementName}'");

                switch (elementName)
                {
                    case "summary":
                        member.Summary = ReadTextBlock(element);
                        break;

                    case "remarks":
                        member.Remarks = ReadTextBlock(element);
                        break;

                    case "example":
                        member.Example = ReadTextBlock(element);
                        break;

                    case "param":
                        {
                            if (element.TryGetAttributeValue("name", out var name))
                                member.Parameters.Add(name, ReadTextBlock(element));
                        }
                        break;

                    case "typeparam":
                        {
                            if (element.TryGetAttributeValue("name", out var name))
                                member.TypeParameters.Add(name, ReadTextBlock(element));
                        }
                        break;

                    case "seealso":
                        {
                            if (TryParseMemberId(element.Attribute("cref")?.Value, out var memberId))
                                member.SeeAlso.Add(new SeeAlsoElement(memberId, ReadTextBlock(element)));
                        }
                        break;

                    case "exception":
                        ReadExceptionElement(element, member);
                        break;

                    case "value":
                        member.Value = ReadTextBlock(element);
                        break;

                    case "returns":
                        member.Returns = ReadTextBlock(element);
                        break;

                    // ignore unknown elements
                    default:
                        m_Logger.LogWarning($"Encountered unknown element '{elementName}'. Element will be ignored.");
                        break;
                }
            }
        }

        private void ReadExceptionElement(XElement element, MemberElement member)
        {
            var cref = element.Attribute("cref")?.Value;

            if (!TryParseMemberId(cref, out var memberId))
                return;

            if (memberId is TypeId typeId)
            {
                member.Exceptions.Add(new ExceptionElement(typeId, ReadTextBlock(element)));
            }
            else
            {
                m_Logger.LogWarning($"Unexpected member id '{cref}' in 'exception' element. " +
                                    $"Expected id of type {nameof(TypeId)} but was {memberId.GetType().Name}. " +
                                    $"Ignoring exception element.");
            }
        }

        /// <summary>
        /// Reads all supported text elements.
        /// </summary>
        private TextBlock ReadTextBlock(XElement xml)
        {
            var textElements = new List<Element>();

            foreach (var node in xml.Nodes())
            {
                var element = default(Element);
                switch (node)
                {
                    case XElement elementNode:
                        switch (elementNode.Name.LocalName)
                        {
                            case "para":
                                element = new ParaElement(ReadTextBlock(elementNode));
                                break;

                            case "paramref":
                                {
                                    if (elementNode.TryGetAttributeValue("name", out var name))
                                        element = new ParamRefElement(name);
                                }
                                break;

                            case "typeparamref":
                                {
                                    if (elementNode.TryGetAttributeValue("name", out var name))
                                        element = new TypeParamRefElement(name);
                                }
                                break;

                            case "code":
                                // get the "language" attribute. If there is no "language" attribute, use the "lang"
                                // attribute. "lang" is legacy syntax according to SandCastle documentation
                                // http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
                                var languageAttribute = elementNode.Attribute("language") ?? elementNode.Attribute("lang");

                                element = new CodeElement(TrimCode(elementNode.Value), languageAttribute?.Value);
                                break;

                            case "c":
                                element = new CElement(elementNode.Value);
                                break;

                            case "see":
                                if (elementNode.TryGetAttributeValue("cref", out var cref) &&
                                    TryParseMemberId(cref, out var memberId))
                                {
                                    element = new SeeElement(memberId);
                                }
                                break;

                            case "list":
                                element = ReadList(elementNode);
                                break;

                            // ignore unknown elements
                            default:
                                m_Logger.LogWarning($"Encountered unknown element '{elementNode.Name}'. Element will be ignored.");
                                break;
                        }
                        break;

                    case XText textNode:
                        element = new TextElement(TrimText(textNode.Value));
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

        private ListElement ReadList(XElement xml)
        {
            // parse list type
            if (!Enum.TryParse<ListType>(xml.Attribute("type")?.Value, ignoreCase: true, out var listType))
            {
                listType = ListType.None;
            }

            var listHeader = (ListItemElement)null;
            var listItems = new List<ListItemElement>();

            // get list header and list items
            foreach (var node in xml.Nodes())
            {
                if (node is XElement element)
                {
                    if (element.Name.LocalName == "item")
                    {
                        var listItem = ReadListItem(element);

                        if (listItem != default)
                            listItems.Add(listItem);

                        continue;
                    }
                    else if (element.Name.LocalName == "listheader" && listHeader == null)
                    {
                        listHeader = ReadListItem(element);
                        continue;
                    }
                }

                m_Logger.LogWarning($"Encountered unexpected node '{node}' while reading list. Node will be ignored.");
            }

            return new ListElement(listType, listHeader, listItems);
        }

        private ListItemElement ReadListItem(XElement xml)
        {
            TextBlock term = null;
            TextBlock description = null;

            foreach (var node in xml.Nodes())
            {
                // Read term and description from first term/description elements.
                // Ignore subsequent term/description elements
                if (node is XElement element)
                {
                    if (element.Name.LocalName == "term" && term == null)
                    {
                        term = ReadTextBlock(element);
                        continue;
                    }
                    else if (element.Name.LocalName == "description" && description == null)
                    {
                        description = ReadTextBlock(element);
                        continue;
                    }
                }

                m_Logger.LogWarning($"Encountered unexpected node '{node}' while reading list. Node will be ignored.");
            }

            // item without term or description => return null
            if (description == null)
            {
                m_Logger.LogWarning($"Ignoring list item {xml} because no 'description' element was found.");
                return null;
            }
            else
            {
                return new ListItemElement(term, description);
            }
        }

        /// <summary>
        /// Trims the text by removing new lines and trimming the indent.
        /// </summary>
        private static string TrimText(string content) => TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ");

        /// <summary>
        /// Trims the code by removing extra indent.
        /// </summary>
        private static string TrimCode(string content) => TrimLines(content, StringSplitOptions.None, Environment.NewLine);

        private static string TrimLines(string content, StringSplitOptions splitOptions, string joinWith)
        {
            var lines = content.Split(new[] { Environment.NewLine, "\n" }, splitOptions).ToList();

            if (lines.Count == 0)
                return String.Empty;

            // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
            if (lines[0].Trim().Length == 0)
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return String.Empty;

            if (lines[lines.Count - 1].Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return String.Empty;

            // The indent of the first line of content determines the base 
            // indent for all the lines, which we should remove since it's just 
            // a doc gen artifact.
            var indent = lines[0].TakeWhile(c => Char.IsWhiteSpace(c)).Count();
            // Indent in generated XML doc files is greater than 4 always. 
            // This allows us to optimize the case where the author actually placed 
            // whitespace inline in between tags.
            if (indent <= 4 && !String.IsNullOrEmpty(lines[0]) && lines[0][0] != '\t')
                indent = 0;

            return String.Join(joinWith, lines
                .Select(line =>
                {
                    if (String.IsNullOrEmpty(line))
                        return line;
                    else if (line.Length < indent)
                        return String.Empty;
                    else
                        return line.Substring(indent);
                })
                .ToArray());
        }

        private bool TryParseMemberId(string value, out MemberId memberId)
        {
            if (!MemberId.TryParse(value, out memberId))
            {
                m_Logger.LogWarning($"Failed to parse member id '{value}'.");
                return false;
            }
            return true;
        }
    }
}
