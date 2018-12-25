using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Differentiate between properties and indexers, add parameters and overloads section for indexers
    class PropertyPage : SimpleMemberPage<PropertyDocumentation>
    {        
        public override OutputPath OutputPath { get; }            
        

        public PropertyPage(PageFactory pageFactory, string rootOutputPath, PropertyDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {            
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "properties", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Property", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            //TODO: "Value" text from xml documentation
            block.Add(Heading("Property Value", 2));
            block.Add(
                Paragraph(
                    GetTypeNameSpan(Model.Type)
            ));
        }        
    }
}
