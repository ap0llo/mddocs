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
        private readonly XDocument? m_Document;
        private readonly string? m_FileName;
        private readonly IReadOnlyCollection<TypeId> m_OuterTypes;

        public XmlDocsReader(ILogger logger, string fileName, IReadOnlyCollection<TypeId> outerTypes)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            m_OuterTypes = outerTypes ?? throw new ArgumentNullException(nameof(outerTypes));
        }

        public XmlDocsReader(ILogger logger, XDocument document, IReadOnlyCollection<TypeId> outerTypes)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_Document = document ?? throw new ArgumentNullException(nameof(document));
            m_OuterTypes = outerTypes ?? throw new ArgumentNullException(nameof(outerTypes));
        }


        /// <summary>
        /// Reads the specified documentation document and returns list of members.
        /// </summary>
        /// <param name="document">The XML documentation file to read.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the specified parameters is <c>null</c></exception>
        public IReadOnlyList<MemberElement> Read()
        {
            XDocument document;
            if (m_Document == null)
            {
                if (!File.Exists(m_FileName))
                    throw new FileNotFoundException("Could not find documentation file to load.", m_FileName);

                m_Logger.LogInformation($"Reading XML documentation comments from '{m_FileName}'");

                document = XDocument.Load(m_FileName!, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            }
            else
            {
                document = m_Document;
            }

            IReadOnlyList<MemberElement>? members = document.Root
                ?.Element("members")
                ?.Elements("member")
                ?.Where(element => element.Attribute("name") != null)
                ?.Select(element => TryReadMember(element))
                ?.Where(x => x != null)
                ?.Select(x => x!)
                ?.ToList();

            return members ?? Array.Empty<MemberElement>();

        }


        /// <summary>
        /// Reads all documentation for a single member
        /// </summary>
        private MemberElement? TryReadMember(XElement element)
        {
            var name = element.Attribute("name")?.Value;

            if (name == null || !TryParseMemberId(name, out var id))
            {
                return null;
            }

            var memberElement = new MemberElement(id!);

            ReadMemberContent(element, memberElement);

            return memberElement;
        }


        /// <summary>
        /// Reads all supported documentation elements and adds it to the specified member.
        /// </summary>
        /// <remarks>
        /// Member is <c>internal</c> for testing purposes only.
        /// </remarks>
        internal void ReadMemberContent(XElement xml, MemberElement member)
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
                                member.Parameters.Add(name!, ReadTextBlock(element));
                        }
                        break;

                    case "typeparam":
                        {
                            if (element.TryGetAttributeValue("name", out var name))
                                member.TypeParameters.Add(name!, ReadTextBlock(element));
                        }
                        break;

                    case "seealso":
                        {
                            // <seealso /> allows adding links to the documentation
                            //
                            //   - using  <seealso cref="..." /> a link to other assembly members
                            //     can be inserted (supported by Visual Studio)
                            //   - using <seealso href="..." /> a link to an external resource,
                            //     typically a website can be specified (unofficial extension, not supported by VS)
                            //
                            //   If both cref and href attributes are present, href is ignored
                            //

                            if (element.TryGetAttributeValue("cref", out var cref))
                            {
                                if (TryParseMemberId(cref, out var memberId))
                                {
                                    member.SeeAlso.Add(new SeeAlsoElement(memberId!, ReadTextBlock(element)));
                                }
                            }
                            else if (element.TryGetAttributeValue("href", out var href))
                            {
                                if (Uri.TryCreate(href, UriKind.Absolute, out var target))
                                {
                                    member.SeeAlso.Add(new SeeAlsoElement(target, ReadTextBlock(element)));
                                }
                            }
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
                                    $"Expected id of type {nameof(TypeId)} but was {memberId!.GetType().Name}. " +
                                    $"Ignoring exception element.");
            }
        }

        /// <summary>
        /// Reads all supported text elements.
        /// </summary>
        /// <remarks>
        /// Member is <c>internal</c> for testing purposes only.
        /// </remarks>
        internal TextBlock ReadTextBlock(XElement xml)
        {
            var textElements = new List<Element>();
            var indent = default(int?);

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
                                        element = new ParamRefElement(name!);
                                }
                                break;

                            case "typeparamref":
                                {
                                    if (elementNode.TryGetAttributeValue("name", out var name))
                                        element = new TypeParamRefElement(name!);
                                }
                                break;

                            case "code":
                                // get the "language" attribute. If there is no "language" attribute, use the "lang"
                                // attribute. "lang" is legacy syntax according to SandCastle documentation
                                // http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
                                var languageAttribute = elementNode.Attribute("language") ?? elementNode.Attribute("lang");

                                element = new CodeElement(TrimCode(elementNode.Value, null), languageAttribute?.Value);
                                break;

                            case "c":
                                element = new CElement(elementNode.Value);
                                break;

                            case "see":
                                // <see /> allows adding links to the documentation
                                //
                                //   - using  <see cref="..." /> a link to other assembly members
                                //     can be inserted (supported by Visual Studio)
                                //   - using <see href="..." /> a link to an external resource,
                                //     typically a website can be specified (unofficial extension, not supported by VS)
                                //
                                //   If both cref and href attributes are present, href is ignored
                                //
                                if (elementNode.TryGetAttributeValue("cref", out var cref))
                                {
                                    if (TryParseMemberId(cref, out var memberId))
                                    {
                                        element = new SeeElement(memberId!, ReadTextBlock(elementNode));
                                    }
                                }
                                else if (elementNode.TryGetAttributeValue("href", out var href))
                                {
                                    if (Uri.TryCreate(href, UriKind.Absolute, out var target))
                                    {
                                        element = new SeeElement(target, ReadTextBlock(elementNode));
                                    }
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
                        // determine the indentation of text in the XML document
                        // when the first text element is encountered.
                        // The indentation needs to be removed from all lines in the current text block
                        indent = indent ?? DetermineIndentation(textNode.Value);
                        element = new TextElement(TrimText(textNode.Value, indent));
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

            ListItemElement? listHeader = default;
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

        private ListItemElement? ReadListItem(XElement xml)
        {
            TextBlock? term = null;
            TextBlock? description = null;

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
        private static string TrimText(string content, int? indent) => TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ", indent);

        /// <summary>
        /// Trims the code by removing extra indent.
        /// </summary>
        private static string TrimCode(string content, int? indent) => TrimLines(content, StringSplitOptions.None, Environment.NewLine, indent);

        private static string TrimLines(string content, StringSplitOptions splitOptions, string joinWith, int? indent)
        {
            // If no indent was specified, calculate it now
            if (indent == null)
            {
                indent = DetermineIndentation(content);
            }

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

            // Indent in generated XML doc files is greater than 4 always. 
            // This allows us to optimize the case where the author actually placed 
            // whitespace inline in between tags.
            if (indent <= 4 && !String.IsNullOrEmpty(lines[0]) && lines[0][0] != '\t')
                indent = 0;

            return String.Join(joinWith, lines
                .Select(line =>
                {
                    if (String.IsNullOrWhiteSpace(line))
                        return String.Empty;
                    // remove indentation from line, when it starts with <indent> whitespace characters
                    else if (indent! <= line.Length && line.Substring(0, indent!.Value).Trim().Length == 0)
                        return line.Substring(indent.Value);
                    // line has non-whitespace content within the indentation => return it unchanged
                    else
                        return line;
                })
                .ToArray());
        }

        /// <summary>
        /// Attempts to get the number of character the text is indented with.
        /// </summary>
        private static int? DetermineIndentation(string content)
        {
            //  When inline documentation is written to a XML file by the compiler, 
            //  all text is indented by the same number of character:
            //
            //  Entire block is indented
            //    │
            //    │    <member name="F:DemoProject.DemoClass.Field1">             Empty leading line  
            //    │       <remarks>                                         <───────┘                        
            //    └───>     Remarks allow specification of more detailed information about a member, in this case a field
            //              supplementing the information specified in the summary
            //  ┌───>     </remarks>
            //  │     </member>
            //  │
            //  │
            //  Empty trailing line
            //
            //  In order to properly render the documentation, the leading whitespace
            //  needs to be removed. 
            //  
            //  This methods determines the number of character that have to be removed
            //  from all lines by 
            //    - removing the first line (probably only whitespace, as can be seen in the example above)
            //    - removing the last line (probably only whitespace, as can be seen in the example above)
            //    - counting the number of whitespace characters in the first non-whitespace line
            //


            var lines = content.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (lines.Count == 0)
                return null;

            // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
            if (lines[0].Trim().Length == 0)
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return null;

            if (lines[lines.Count - 1].Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return null;

            // The indent of the first line of content determines the base 
            // indent for all the lines, which we should remove since it's just 
            // a doc gen artifact.            
            return lines[0].TakeWhile(c => Char.IsWhiteSpace(c)).Count();

        }


        private bool TryParseMemberId(string? value, out MemberId? memberId)
        {
            if (!MemberId.TryParse(value, m_OuterTypes, out memberId))
            {
                m_Logger.LogWarning($"Failed to parse member id '{value}'.");
                return false;
            }
            return true;
        }
    }
}
