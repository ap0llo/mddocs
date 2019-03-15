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

        public InternalLinkProvider(IDocumentation model, PageFactory pageFactory)
        {
            m_Model = model;
            m_PageFactory = pageFactory;
        }

        ///<inheritdoc />
        public bool TryGetLink(MemberId id, out Link link)
        {
            var modelItem = m_Model.TryGetDocumentation(id);

            if (modelItem == null)
            {
                link = default;
                return false;
            }

            var page = m_PageFactory.TryGetPage(modelItem);

            if (page == null)
            {
                link = default;
                return false;
            }

            if (page.TryGetAnchor(id, out var anchor))
            {
                link = new Link(page.OutputPath, anchor);
            }
            else
            {
                link = new Link(page.OutputPath);
            }

            return true;
        }
    }
}
