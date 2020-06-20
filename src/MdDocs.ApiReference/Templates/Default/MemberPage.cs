using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {
        internal MemberPage(ILinkProvider linkProvider, ApiReferenceConfiguration configuration, TModel model)
            : base(linkProvider, configuration, model)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            var paragraph = new MdParagraph(
                new MdStrongEmphasisSpan("Declaring Type:"), " ", GetMdSpan(Model.TypeDocumentation.TypeId),
                "\r\n",
                new MdStrongEmphasisSpan("Namespace:"), " ", GetMdSpan(Model.TypeDocumentation.NamespaceDocumentation.NamespaceId),
                "\r\n",
                new MdStrongEmphasisSpan("Assembly:"), " " + Model.GetAssemblyDocumentation().Name
            );

            if (m_Configuration.IncludeVersion)
            {
                paragraph.Add("\r\n");
                paragraph.Add(new MdCompositeSpan(
                    new MdStrongEmphasisSpan("Assembly Version:"), " " + Model.GetAssemblyDocumentation().Version)
                );
            }

            block.Add(paragraph);
        }
    }
}
