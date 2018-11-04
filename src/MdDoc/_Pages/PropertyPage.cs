using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class PropertyPage : PageBase
    {
        private readonly PropertyDefinition m_Property;

        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Property);

        public PropertyPage(DocumentationContext context, PathProvider pathProvider, PropertyDefinition property)
            : base(context, pathProvider)
        {
            m_Property = property ?? throw new ArgumentNullException(nameof(property));
        
        }


        public void SaveDocumentation()
        {
            var document = Document(
                Heading($"{m_Property.DeclaringType.Name}.{m_Property.Name} Property", 1)
            );

            
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
