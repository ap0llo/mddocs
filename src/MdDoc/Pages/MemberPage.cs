using System.Collections.Generic;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {        
        public MemberPage(PageFactory pageFactory, string rootOutputPath) : base(pageFactory, rootOutputPath)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("Declaring Type:"), " ", GetTypeNameSpan(Model.TypeDocumentation.TypeId)
            ));
        } 
    }
}
