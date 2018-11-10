using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class PropertyPage : MemberPage
    {
        private readonly PropertyDocumentation m_Model;



        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Model.Definition);

        protected override TypeReference DeclaringType => m_Model.Definition.DeclaringType;


        public PropertyPage(DocumentationContext context, PathProvider pathProvider, PropertyDocumentation model)
            : base(context, pathProvider)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Definition.DeclaringType.Name}.{m_Model.Definition.Name} Property", 1)
            );

            AddDeclaringTypeSection(document.Root);

            document.Root.Add(
                Paragraph(
                    m_Context.XmlDocProvider.TryGetDocumentation(m_Model.Definition).Summary
            ));

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
