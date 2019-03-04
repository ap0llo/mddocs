using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
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
                GetMdParagraph(Model.Type)
            );

            if (Model.Value != null)
            {
                block.Add(ConvertToBlock(Model.Value));
            }

            // After the value section, add the Exceptions section
            AddExceptionsSection(block);
        }


        protected void AddExceptionsSection(MdContainerBlock block)
        {
            if (Model.Exceptions.Count == 0)
                return;

            block.Add(Heading("Exceptions", 2));

            foreach (var exception in Model.Exceptions)
            {
                block.Add(
                    GetMdParagraph(exception.Type),
                    ConvertToBlock(exception.Text)
                );
            }
        }
    }
}
