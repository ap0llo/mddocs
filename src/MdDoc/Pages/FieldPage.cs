using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class FieldPage : SimpleMemberPage<FieldDocumentation>
    {        
        public override OutputPath OutputPath { get; }            
        

        public FieldPage(PageFactory pageFactory, string rootOutputPath, FieldDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "fields", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }



        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Field", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            //TODO: "Value" text from xml documentation
            block.Add(Heading("Field Value", 2));
            block.Add(
                Paragraph(
                    GetTypeNameSpan(Model.Type)
            ));
        }
    }
}
