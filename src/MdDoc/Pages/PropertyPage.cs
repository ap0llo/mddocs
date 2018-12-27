using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{    
    class PropertyPage : SimpleMemberPage<PropertyDocumentation>
    {        
        public override OutputPath OutputPath { get; }            
        

        public PropertyPage(PageFactory pageFactory, string rootOutputPath, PropertyDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {            
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "Properties", $"{Model.Name}.md"));
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Property", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            block.Add(Heading("Property Value", 2));
            block.Add(
                Paragraph(GetTypeNameSpan(Model.Type)
            ));

            if (Model.Value != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Value, this));
            }
        }        
    }
}
