using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class PropertyPage : MemberPage
    {
        private readonly PropertyDocumentation m_Model;



        public override OutputPath OutputPath =>
            new OutputPath(Path.Combine(GetTypeDir(m_Model.TypeDocumentation.Definition), "properties", $"{m_Model.TypeDocumentation.Name}.{m_Model.Definition.Name}.md"));

        protected override TypeReference DeclaringType => m_Model.Definition.DeclaringType;

        protected override IDocumentation Model => m_Model;


        public PropertyPage(PageFactory pageFactory, string rootOutputPath, PropertyDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Definition.DeclaringType.Name}.{m_Model.Definition.Name} Property", 1)
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
            var hasGetter = m_Model.Definition.GetMethod?.IsPublic == true;
            var hasSetter = m_Model.Definition.SetMethod?.IsPublic == true;

            var definition = $"public {m_Model.Definition.PropertyType.Name} {m_Model.Definition.Name} {{ {(hasGetter ? "get;" : "")} {(hasSetter ? "set;" : "")} }}";

            block.Add(
                CodeBlock(definition, "csharp")
            );
        }

        private void AddPropertyValueSection(MdContainerBlock block)
        {
            block.Add(Heading("Property Value", 2));
            block.Add(
                Paragraph(
                    GetTypeNameSpan(m_Model.Definition.PropertyType)
            ));
        }        
    }
}
