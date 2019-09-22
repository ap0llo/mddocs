using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {
        public MemberPage(ILinkProvider linkProvider, PageFactory pageFactory, TModel model)
            : base(linkProvider, pageFactory, model)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                new MdParagraph(
                    new MdStrongEmphasisSpan("Declaring Type:"), " ", GetMdSpan(Model.TypeDocumentation.TypeId)
            ));
        }
    }
}
