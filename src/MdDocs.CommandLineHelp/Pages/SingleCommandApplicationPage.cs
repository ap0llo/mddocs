using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class SingleCommandApplicationPage : IDocument
    {
        private readonly SingleCommandApplicationDocumentation m_Model;
        private readonly DocumentSet<IDocument> m_DocumentSet;
        private readonly IPathProvider m_PathProvider;


        public SingleCommandApplicationPage(DocumentSet<IDocument> documentSet, IPathProvider pathProvider, SingleCommandApplicationDocumentation model)
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

            // Usage
            document.Add(new UnnamedCommandUsageSection(m_Model.Parameters));
            // Parameters
            document.AddIf(m_Model.Parameters.Parameters.Count > 0, () => new CommandParametersSection(m_Model.Parameters));
            // Footer
            document.Add(new PageFooter());


            return document;
        }


       
    }
}
