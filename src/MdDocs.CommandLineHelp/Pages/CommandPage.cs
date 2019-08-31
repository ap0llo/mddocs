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
            if(m_Model.Options.Any(x => !x.Hidden))
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

            var first = true;
            foreach (var option in m_Model.Options.Where(x => !x.Hidden))
            {
                if (m_Model.Options.Count > 1 && !first)
                {
                    block.Add(new MdThematicBreak());
                }

                block.Add(GetParameterSection(option));
                first = false;
            }

            return block;
        }

        private MdTable GetParametersTable()
        {
            var addNameColumn = m_Model.Options.Any(x => !x.Hidden && !String.IsNullOrEmpty(x.Name));
            var addShortNameColumn = m_Model.Options.Any(x => !x.Hidden && x.ShortName != null);

            var headerRow = new MdTableRow();

            if (addNameColumn)
            {
                headerRow.Add("Name");
            }

            if (addShortNameColumn)
            {
                headerRow.Add("Short Name");
            }

            headerRow.Add("Description");

            var table = new MdTable(headerRow);

            foreach(var option in m_Model.Options.Where(x => !x.Hidden))
            {
                var row = new MdTableRow();

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

        private static MdHeading GetHeading(OptionDocumentation option)
        {
            var name = option.Name ?? option.ShortName.ToString();
            return new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name), " Parameter"));
        }
    }
}
