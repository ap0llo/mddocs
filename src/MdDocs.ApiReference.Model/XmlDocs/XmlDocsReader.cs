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
    internal static class XmlDocsReader
    {
        // SandCastle language ids, see http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
        private static readonly IDictionary<string, CodeLanguage> s_SandCastleLanguageIds = new Dictionary<string, CodeLanguage>(StringComparer.OrdinalIgnoreCase)
        {
            { "cs", CodeLanguage.CSharp },
            { "c#", CodeLanguage.CSharp },
            { "csharp", CodeLanguage.CSharp },
            { "cpp", CodeLanguage.CPlusPlus },
            { "cpp#", CodeLanguage.CPlusPlus },
            { "C++", CodeLanguage.CPlusPlus },
            { "CPlusPlus", CodeLanguage.CPlusPlus },
            { "c", CodeLanguage.C },
            { "fs", CodeLanguage.FSharp },
            { "f#", CodeLanguage.FSharp },
            { "FSharp", CodeLanguage.FSharp },
            { "fscript", CodeLanguage.FSharp },
            { "EcmaScript", CodeLanguage.Javascript },
            { "js", CodeLanguage.Javascript },
            { "JavaScript", CodeLanguage.Javascript },
            { "jscript", CodeLanguage.JScriptDotNet },
            { "jscript#", CodeLanguage.JScriptDotNet },
            { "jscriptnet", CodeLanguage.JScriptDotNet },
            { "JScript.NET", CodeLanguage.JScriptDotNet },
            { "VB", CodeLanguage.VisualBasic },
            { "VB#", CodeLanguage.VisualBasic },
            { "vbnet", CodeLanguage.VisualBasic },
            { "VB.NET", CodeLanguage.VisualBasic },
            { "vbs", CodeLanguage.VisualBasicScript },
            { "vbscript", CodeLanguage.VisualBasicScript },
            { "htm", CodeLanguage.HTML },
            { "html", CodeLanguage.HTML },
            { "xml", CodeLanguage.XML },
            { "xsl", CodeLanguage.XML },
            { "xaml", CodeLanguage.XAML },
            { "jsharp", CodeLanguage.JSharp },
            { "J#", CodeLanguage.JSharp },
            { "sql", CodeLanguage.SQL },
            { "sql server", CodeLanguage.SQL },
            { "sqlserver", CodeLanguage.SQL },
            { "py", CodeLanguage.Python },
            { "python", CodeLanguage.Python },
            { "pshell", CodeLanguage.Powershell },
            { "powershell", CodeLanguage.Powershell },
            { "ps1", CodeLanguage.Powershell },
            { "bat", CodeLanguage.Batch },
            { "batch", CodeLanguage.Batch },

        };



        /// <summary>
        /// Reads the specified documentation file and returns list of members.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static IReadOnlyList<MemberElement> Read(string fileName, ILogger logger)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            logger.LogInformation($"Reading XML documentation comments from '{fileName}'");

            var doc = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => ReadMember(element.Attribute("name").Value, element, logger))
                .ToList();
        }


        /// <summary>
        /// Reads all documentation for a single member
        /// </summary>
        private static MemberElement ReadMember(string memberId, XElement element, ILogger logger)
        {
            MemberId id;
            try
            {
                id = MemberId.Parse(memberId);
            }
            catch (MemberIdParserException ex)
            {
                logger.LogCritical(ex, $"Failed to parse member id '{memberId}'");
                throw;
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
                            if (cref != null)
                            {
                                var memberId = MemberId.Parse(cref);
                                member.SeeAlso.Add(new SeeAlsoElement(memberId, ReadTextBlock(elementNode, logger)));
                            }
                        }
                        break;
                    case "exception":
                        {
                            var cref = FindAttribute(elementNode, "cref");
                            if (cref != null)
                            {
                                MemberId memberId;
                                try
                                {
                                    memberId = MemberId.Parse(cref);
                                }
                                catch (MemberIdParserException ex)
                                {
                                    logger.LogError(ex, $"Failed to parse member id '{cref}' in element 'exception'. Ignoring exception element.");
                                    break;
                                }

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

                                var codeLanguage = languageAttribute != null
                                    ? GetCodeLanguage(languageAttribute.Value, logger)
                                    : CodeLanguage.None;

                                element = new CodeElement(TrimCode(elementNode.Value), codeLanguage);
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

   
        internal static CodeLanguage GetCodeLanguage(string languageName, ILogger logger)
        {
            if (Enum.TryParse<CodeLanguage>(languageName, ignoreCase: true, out var codeLanguage))
            {
                return codeLanguage;
            }

            if(s_SandCastleLanguageIds.TryGetValue(languageName, out codeLanguage))
            {
                return codeLanguage;
            }

            logger.LogWarning($"Unrecognized language '{languageName}' in code element.");
            return CodeLanguage.None;
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

#pragma warning restore IDE0049 // Use framework type
