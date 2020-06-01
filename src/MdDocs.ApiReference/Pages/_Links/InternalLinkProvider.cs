using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    /// <summary>
    /// Link provider that provides links to documentation pages within the current set of pages.
    /// </summary>
    internal class InternalLinkProvider : ILinkProvider
    {
        private readonly IDocumentation m_Model;
        private readonly PageFactory m_PageFactory;
        private readonly DocumentSet<IDocument> m_DocumentSet;

        public InternalLinkProvider(IDocumentation model, PageFactory pageFactory, DocumentSet<IDocument> documentSet)
        {
            m_Model = model;
            m_PageFactory = pageFactory;
            m_DocumentSet = documentSet;
        }

        public bool TryGetLink(IDocument from, MemberId id, out Link? link)
        {
            // map id -> model item
            var modelItem = m_Model.TryGetDocumentation(id);

            if (modelItem == null)
            {
                link = default;
                return false;
            }

            // map model  -> page
            var page = m_PageFactory.TryGetPage(modelItem);

            if (page == null)
            {
                link = default;
                return false;
            }

            // get link between pages
            var relativePath = m_DocumentSet.GetRelativePath(from, page);

            if (page.TryGetAnchor(id, out var anchor))
            {
                link = new Link(relativePath, anchor);
            }
            else
            {
                link = new Link(relativePath);
            }

            return true;
        }
    }
}
