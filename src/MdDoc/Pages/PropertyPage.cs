using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Add documentation from XML comments
    class PropertyPage : MemberPage<PropertyDocumentation>
    {        
        public override OutputPath OutputPath { get; }            


        protected override TypeReference DeclaringType => Model.Definition.DeclaringType;

        protected override PropertyDocumentation Model { get; }


        public PropertyPage(PageFactory pageFactory, string rootOutputPath, PropertyDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "properties", $"{Model.TypeDocumentation.Name}.{Model.Definition.Name}.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.Definition.DeclaringType.Name}.{Model.Name} Property", 1)
            );

            AddDeclaringTypeSection(document.Root);

            AddDefinitionSection(document.Root);

            AddPropertyValueSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddDefinitionSection(MdContainerBlock block)
        {            
            block.Add(
                CodeBlock(Model.CSharpDefinition, "csharp")
            );
        }

        private void AddPropertyValueSection(MdContainerBlock block)
        {
            block.Add(Heading("Property Value", 2));
            block.Add(
                Paragraph(
                    GetTypeNameSpan(Model.Definition.PropertyType)
            ));
        }        
    }
}
