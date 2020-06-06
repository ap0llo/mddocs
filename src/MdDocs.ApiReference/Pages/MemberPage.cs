using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {
        internal MemberPage(ILinkProvider linkProvider, ApiReferenceConfiguration configuration, TModel model)
            : base(linkProvider, configuration, model)
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
