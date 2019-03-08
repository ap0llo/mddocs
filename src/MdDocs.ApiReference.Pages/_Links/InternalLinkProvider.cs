using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    class InternalLinkProvider : ILinkProvider
    {
        private readonly IDocumentation m_Model;
        private readonly PageFactory m_PageFactory;

        public InternalLinkProvider(IDocumentation model, PageFactory pageFactory)
        {
            m_Model = model;
            m_PageFactory = pageFactory;
        }

        public bool TryGetLink(MemberId id, out string link)
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

            link = page.OutputPath;
            return true;
        }
    }
}
