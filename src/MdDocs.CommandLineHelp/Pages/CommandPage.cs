using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class CommandPage : IDocument
    {
        private readonly CommandDocumentation m_Model;


        public CommandPage(CommandDocumentation model)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public void Save(string path) => GetDocument().Save(path);


        internal MdDocument GetDocument()
        {
            var document = new MdDocument();

            // Heading
            document.Root.Add(new MdHeading(1, new MdCompositeSpan(new MdCodeSpan(m_Model.Name), " Command")));

            // Help text
            if (!String.IsNullOrEmpty(m_Model.HelpText))
            {
                document.Root.Add(new MdParagraph(m_Model.HelpText));
            }

            // Parameters
            if(m_Model.Parameters.Any(x => !x.Hidden))
            {
                document.Root.Add(GetParametersSection());
            }

            return document;
        }


        private MdContainerBlock GetParametersSection()
        {
            var block = new MdContainerBlock
            {
                new MdHeading(2, "Parameters"),
                GetParametersTable()
            };

            var sections = Enumerable.Concat(
                m_Model.Values.Where(x => !x.Hidden).Select(GetParameterSection),
                m_Model.Options.Where(x => !x.Hidden).Select(GetParameterSection)
            ).ToArray() ;


            var first = true;
            foreach(var section in sections)
            {
                if (sections.Length > 1 && !first)
                {
                    block.Add(new MdThematicBreak());
                }

                block.Add(section);
                first = false;
            }

            return block;
        }

        private MdTable GetParametersTable()
        {
            var addNameColumn = m_Model.Options.Any(x => !x.Hidden && !String.IsNullOrEmpty(x.Name));
            var addShortNameColumn = m_Model.Options.Any(x => !x.Hidden && x.ShortName != null);
            var addPositionColumn = m_Model.Values.Count > 0;
            
            var headerRow = new MdTableRow();

            if(addPositionColumn)
            {
                headerRow.Add("Position");
            }

            if (addNameColumn)
            {
                headerRow.Add("Name");
            }

            if (addShortNameColumn && !addNameColumn)
            {
                headerRow.Add("Name");
            }
            else if (addShortNameColumn)
            {
                headerRow.Add("Short Name");
            }

            headerRow.Add("Description");

            var table = new MdTable(headerRow);


            foreach(var value in m_Model.Values.Where(x => !x.Hidden).OrderBy(x => x.Index))
            {
                var row = new MdTableRow();

                row.Add(value.Index.ToString());

                var anchor = "#" + GetHeading(value).Anchor;
                if (addNameColumn)
                {
                    if (String.IsNullOrWhiteSpace(value.Name))
                    {
                        row.Add("");
                    }
                    else
                    {
                        row.Add(new MdLinkSpan(value.Name ?? "", anchor));
                    }
                }

                if (addShortNameColumn)
                {
                    if(String.IsNullOrEmpty(value.Name))
                    {
                        row.Add("");
                    }
                    else if(!addNameColumn)
                    {
                        row.Add(new MdLinkSpan(value.Name ?? "", anchor));
                    }
                    else
                    {
                        row.Add("");
                    }
                }

                row.Add(value.HelpText ?? "");

                table.Add(row);
            }


            foreach(var option in m_Model.Options.Where(x => !x.Hidden))
            {
                var row = new MdTableRow();

                if(addPositionColumn)
                {
                    row.Add("");
                }

                var anchor = "#" + GetHeading(option).Anchor;
                if (addNameColumn)
                {
                    if(String.IsNullOrWhiteSpace(option.Name))
                    {
                        row.Add("");
                    }
                    else
                    {
                        row.Add(new MdLinkSpan(option.Name ?? "", anchor));
                    }
                }

                if (addShortNameColumn)
                {
                    if(option.ShortName == null)
                    {
                        row.Add("");
                    }
                    else
                    {
                        row.Add(new MdLinkSpan(option.ShortName?.ToString() ?? "", anchor));
                    }
                }

                row.Add(option.HelpText ?? "");

                table.Add(row);
            }

            return table;
        }

        private MdContainerBlock GetParameterSection(OptionDocumentation option)
        {
            var block = new MdContainerBlock
            {
                GetHeading(option)
            };

            if (!String.IsNullOrEmpty(option.HelpText))
            {
                block.Add(new MdParagraph(option.HelpText));
            }

            if(option.Default != null)
            {
                block.Add(new MdParagraph(
                    new MdStrongEmphasisSpan("Default value:"),
                    " ",
                    new MdCodeSpan(Convert.ToString(option.Default))
                ));
            }

            return block;
        }
      
        private MdContainerBlock GetParameterSection(ValueDocumentation value)
        {
            var block = new MdContainerBlock
            {
                GetHeading(value)
            };

            if (!String.IsNullOrEmpty(value.HelpText))
            {
                block.Add(new MdParagraph(value.HelpText));
            }

            if (value.Default != null)
            {
                block.Add(new MdParagraph(
                    new MdStrongEmphasisSpan("Default value:"),
                    " ",
                    new MdCodeSpan(Convert.ToString(value.Default))
                ));
            }

            return block;
        }

        private static MdHeading GetHeading(OptionDocumentation option)
        {
            var name = option.Name ?? option.ShortName.ToString();
            return new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name), " Parameter"));
        }

        private static MdHeading GetHeading(ValueDocumentation value)
        {
            if(!String.IsNullOrEmpty(value.Name))
            {
                return new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(value.Name), $" Parameter (Position {value.Index})"));
            }
            else
            {
                //TODO: Find a better heading
                return new MdHeading(3, new MdCompositeSpan($"Parameter (Position {value.Index})"));
            }            
        }
    }
}
