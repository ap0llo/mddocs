using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {
        public MemberPage(PageFactory pageFactory, string rootOutputPath, TModel model)
            : base(pageFactory, rootOutputPath, model)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("Declaring Type:"), " ", GetMdSpan(Model.TypeDocumentation.TypeId)
            ));
        }
    }
}
