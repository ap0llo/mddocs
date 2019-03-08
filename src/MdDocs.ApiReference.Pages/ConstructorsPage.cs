using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    class ConstructorsPage : OverloadableMemberPage<ConstructorDocumentation, ConstructorOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Constructors.md");
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName} Constructors", 1);


        //No "Returns" subsection for constructors
        protected override void AddReturnsSubSection(MdContainerBlock block, ConstructorOverloadDocumentation overload)
        { }
    }
}
