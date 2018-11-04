using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class ConstructorsPage : MemberPage
    {
        private readonly TypeDefinition m_Type;

        public override string Name => $"{m_Type.Name} Constructors";

        protected override OutputPath OutputPath => m_PathProvider.GetConstructorsOutputPath(m_Type);

        protected override TypeReference DeclaringType => m_Type;

        
        public ConstructorsPage(DocumentationContext context, PathProvider pathProvider, TypeDefinition type)
            : base(context, pathProvider)
        {
            m_Type = type ?? throw new ArgumentNullException(nameof(type));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Type.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, m_Type.GetDocumentedConstrutors(m_Context));

            AddDetailSections(document.Root, m_Type.GetDocumentedConstrutors(m_Context));

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }        
    }
}
