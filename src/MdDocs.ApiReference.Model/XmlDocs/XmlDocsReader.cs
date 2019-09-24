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

#pragma warning disable IDE0049 // Use framework type

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;

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
    internal static class XmlDocsReader
    {
        /// <summary>
        /// Reads the specified documentation file and returns list of members.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// /// <param name="logger">The logger to use for the operation.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the specified parameters is <c>null</c></exception>
        public static IReadOnlyList<MemberElement> Read(string fileName, ILogger logger)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            logger.LogInformation($"Reading XML documentation comments from '{fileName}'");

            var document = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return Read(document, logger);            
        }

        /// <summary>
        /// Reads the specified documentation document and returns list of members.
        /// </summary>
        /// <param name="document">The XML documentation file to read.</param>
        /// <param name="logger">The logger to use for the operation.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the specified parameters is <c>null</c></exception>
        public static IReadOnlyList<MemberElement> Read(XDocument document, ILogger logger)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            return document.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => TryReadMember(element, logger))
                .Where(x => x != null)
                .ToList();
        }


        /// <summary>
        /// Reads all documentation for a single member
        /// </summary>
        private static MemberElement TryReadMember(XElement element, ILogger logger)
        {
            var name = element.Attribute("name").Value;

            if(!TryParseMemberId(logger, name, out var id))
            {
                return null;
            }

            var memberElement = new MemberElement(id);

            ReadMemberContent(element, memberElement, logger);

            return memberElement;
        }


        /// <summary>
        /// Reads all supported documentation elements and adds it to the specified member.
        /// </summary>
        private static void ReadMemberContent(XElement xml, MemberElement member, ILogger logger)
        {
            foreach (var elementNode in xml.Elements())
            {
                var elementName = elementNode.Name.LocalName;

                logger.LogDebug($"Reading XML documentation element '{elementName}'");

                switch (elementName)
                {
                    case "summary":
                        member.Summary = ReadTextBlock(elementNode, logger);
                        break;
                    case "remarks":
                        member.Remarks = ReadTextBlock(elementNode, logger);
                        break;
                    case "example":
                        member.Example = ReadTextBlock(elementNode, logger);
                        break;
                    case "param":
                        {
                            var name = FindAttribute(elementNode, "name");
                            if (name != null)
                                member.Parameters.Add(name, ReadTextBlock(elementNode, logger));
                        }
                        break;
                    case "typeparam":
                        {
                            var name = FindAttribute(elementNode, "name");
                            if (name != null)
                                member.TypeParameters.Add(name, ReadTextBlock(elementNode, logger));
                        }
                        break;
                    case "seealso":
                        {
                            var cref = FindAttribute(elementNode, "cref");
                            if (cref != null && TryParseMemberId(logger, cref, out var memberId))
                            {
                                member.SeeAlso.Add(new SeeAlsoElement(memberId, ReadTextBlock(elementNode, logger)));
                            }
                        }
                        break;
                    case "exception":
                        {
                            var cref = FindAttribute(elementNode, "cref");
                            if (cref != null && TryParseMemberId(logger, cref, out var memberId))
                            {
                                if (memberId is TypeId typeId)
                                {
                                    member.Exceptions.Add(
                                        new ExceptionElement(typeId, ReadTextBlock(elementNode, logger))
                                    );
                                }
                                else
                                {
                                    logger.LogWarning($"Unexpected member id '{cref}' in 'exception' element. " +
                                                      $"Expected id of type {nameof(TypeId)} but was {memberId.GetType().Name}. " +
                                                      $"Ignoring exception element.");
                                }
                            }
                        }
                        break;
                    case "value":
                        member.Value = ReadTextBlock(elementNode, logger);
                        break;
                    case "returns":
                        member.Returns = ReadTextBlock(elementNode, logger);
                        break;

                    // ignore unknown elements
                    default:
                        logger.LogWarning($"Encountered unknown element '{elementName}'. Element will be ignored.");
                        break;
                }
            }
        }

        /// <summary>
        /// Reads all supported text elements.
        /// </summary>
        private static TextBlock ReadTextBlock(XElement xml, ILogger logger)
        {
            var textElements = new List<Element>();

            foreach (var node in xml.Nodes())
            {
                var element = default(Element);
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        var elementName = elementNode.Name.LocalName;
                        switch (elementName)
                        {
                            case "para":
                                element = new ParaElement(ReadTextBlock(elementNode, logger));
                                break;
                            case "paramref":
                                element = new ParamRefElement(FindAttribute(elementNode, "name"));
                                break;
                            case "typeparamref":
                                element = new TypeParamRefElement(FindAttribute(elementNode, "name"));
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
                                if(TryParseMemberId(logger, FindAttribute(elementNode, "cref"), out var memberId))
                                {
                                    element = new SeeElement(memberId);
                                }
                                break;
                            case "list":                                                                
                                element = ReadList(elementNode, logger);                                
                                break;
                            
                            // ignore unknown elements
                            default:
                                logger.LogWarning($"Encountered unknown element '{elementName}'. Element will be ignored.");
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

        private static ListElement ReadList(XElement xml, ILogger logger)
        {
            // parse list type
            if (!Enum.TryParse<ListType>(FindAttribute(xml, "type"), ignoreCase: true, out var listType))
            {
                listType = ListType.None;
            }

            var listHeader = (ListItemElement)null;
            var listItems = new List<ListItemElement>();

            // get list header and list items
            foreach (var node in xml.Nodes())
            {
                if(node is XElement element)
                {
                    if(element.Name.LocalName == "item")
                    {
                        var listItem = ReadListItem(element, logger);

                        if(listItem != default)
                            listItems.Add(listItem);

                        continue;
                    }
                    else if(element.Name.LocalName == "listheader" && listHeader == null)
                    {
                        listHeader = ReadListItem(element, logger);
                        continue;
                    }
                }

                logger.LogWarning($"Encountered unexpected node '{node}' while reading list. Node will be ignored.");
            }

            return new ListElement(listType, listHeader, listItems);
        }

        private static ListItemElement ReadListItem(XElement xml, ILogger logger)
        {
            TextBlock term = null;
            TextBlock description = null;

            foreach(var node in xml.Nodes())
            {
                // Read term and description from first term/description elements.
                // Ignore subsequent term/description elements
                if(node is XElement element)
                {
                    if (element.Name.LocalName == "term" && term == null)
                    {
                        term = ReadTextBlock(element, logger);
                        continue;
                    }
                    else if(element.Name.LocalName == "description" && description == null)
                    {
                        description = ReadTextBlock(element, logger);
                        continue;
                    }
                }

                logger.LogWarning($"Encountered unexpected node '{node}' while reading list. Node will be ignored.");
            }

            // item without term or description => return null
            if(description == null)
            {
                logger.LogWarning($"Ignoring list item {xml} because no 'description' element was found.");
                return null;
            }
            else
            {
                return new ListItemElement(term, description);
            }
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

        private static bool TryParseMemberId(ILogger logger, string value, out MemberId memberId)
        {
            try
            {
                memberId = MemberId.Parse(value);
                return true;
            }
            catch (MemberIdParserException ex)
            {
                logger.LogWarning(ex, $"Failed to parse member id '{value}'.");
                memberId = default;
                return false;
            }
        }
    }
}

#pragma warning restore IDE0049 // Use framework type
