using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public OperatorPage(PageFactory pageFactory, string rootOutputPath, OperatorDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Operators", $"{Model.Kind}.md");
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
