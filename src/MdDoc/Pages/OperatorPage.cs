using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
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
