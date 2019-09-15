using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class ApplicationPage : IDocument
    {
        private readonly ApplicationDocumentation m_Model;
        private readonly DocumentSet<IDocument> m_DocumentSet;
        private readonly IPathProvider m_PathProvider;


        public ApplicationPage(DocumentSet<IDocument> documentSet, IPathProvider pathProvider, ApplicationDocumentation model)
        {
            m_DocumentSet = documentSet ?? throw new ArgumentNullException(nameof(documentSet));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public void Save(string path) => GetDocument().Save(path);


        internal MdDocument GetDocument()
        {
            var document = new MdDocument();

            document.Add(new MdHeading(1, $"{m_Model.Name} Command Line Reference"));

            if (!String.IsNullOrEmpty(m_Model.Version))
            {
                document.Add(
                    new MdParagraph(
                        new MdStrongEmphasisSpan("Version:"),
                        " ",
                        m_Model.Version));
            }

            if (m_Model.Usage.Count > 0)
            {
                document.Add(new MdHeading(2, "Usage"));
                document.Add(new MdParagraph(
                    String.Join(Environment.NewLine, m_Model.Usage)
                ));
            }

            if (m_Model.Commands.Count > 0)
            {
                document.Add(new MdHeading(2, "Commands"));
                document.Add(GetCommandsTable());
            }

            document.Root.Add(new PageFooter());

            return document;
        }


        private MdTable GetCommandsTable()
        {
            var table = new MdTable(new MdTableRow("Name", "Description"));
            foreach (var command in m_Model.Commands)
            {
                var commandPage = m_DocumentSet[m_PathProvider.GetPath(command)];

                var link = m_DocumentSet.GetLink(this, commandPage, command.Name);

                table.Add(new MdTableRow(link, command.HelpText ?? ""));
            }
            return table;
        }
    }
}
