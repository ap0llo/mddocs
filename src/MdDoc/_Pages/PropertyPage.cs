using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class PropertyPage : MemberPage
    {
        private readonly PropertyDefinition m_Property;


        public override string Name => $"{m_Property.DeclaringType.Name}.{m_Property.Name} Property";

        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Property);

        protected override TypeReference DeclaringType => m_Property.DeclaringType;


        public PropertyPage(DocumentationContext context, PathProvider pathProvider, PropertyDefinition property)
            : base(context, pathProvider)
        {
            m_Property = property ?? throw new ArgumentNullException(nameof(property));
        
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Property.DeclaringType.Name}.{m_Property.Name} Property", 1)
            );

            AddDeclaringTypeSection(document.Root);

            document.Root.Add(
                Paragraph(
                    m_Context.XmlDocProvider.TryGetDocumentation(m_Property).Summary
            ));

            AddDefinitionSection(document.Root);

            AddPropertyValueSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddDefinitionSection(MdContainerBlock block)
        {
            var hasGetter = m_Property.GetMethod?.IsPublic == true;
            var hasSetter = m_Property.SetMethod?.IsPublic == true;

            var definition = $"public {m_Property.PropertyType.Name} {m_Property.Name} {{ {(hasGetter ? "get;" : "")} {(hasSetter ? "set;" : "")} }}";

            block.Add(
                CodeBlock(definition, "csharp")
            );
        }

        private void AddPropertyValueSection(MdContainerBlock block)
        {
            block.Add(Heading("Property Value", 2));
            block.Add(
                Paragraph(
                    GetTypeNameSpan(m_Property.PropertyType)
            ));
        }
    }
}
