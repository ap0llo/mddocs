using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
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
                Heading($"{Model.Definition.DeclaringType.Name}.{Model.Definition.Name} Property", 1)
            );

            AddDeclaringTypeSection(document.Root);

            //document.Root.Add(
            //    Paragraph(
            //        m_Context.XmlDocProvider.TryGetDocumentation(m_Model.Definition).Summary
            //));

            AddDefinitionSection(document.Root);

            AddPropertyValueSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddDefinitionSection(MdContainerBlock block)
        {
            var hasGetter = Model.Definition.GetMethod?.IsPublic == true;
            var hasSetter = Model.Definition.SetMethod?.IsPublic == true;

            var definition = $"public {Model.Definition.PropertyType.Name} {Model.Definition.Name} {{ {(hasGetter ? "get;" : "")} {(hasSetter ? "set;" : "")} }}";

            block.Add(
                CodeBlock(definition, "csharp")
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
