using Grynwald.MarkdownGenerator;
using MdDoc.Model;
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
        private readonly MethodDocumentation m_Model;

        public override string Name => $"{m_Model.Overloads.First().DeclaringType.Name} Constructors";

        protected override OutputPath OutputPath => m_PathProvider.GetConstructorsOutputPath(m_Model.Overloads.First().DeclaringType);

        protected override TypeReference DeclaringType => m_Model.Overloads.First().DeclaringType;

        
        public ConstructorsPage(DocumentationContext context, PathProvider pathProvider, MethodDocumentation model)
            : base(context, pathProvider)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Overloads.First().DeclaringType.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, m_Model.Overloads.First().DeclaringType.GetDocumentedConstrutors(m_Context));

            AddDetailSections(document.Root, m_Model.Overloads.First().DeclaringType.GetDocumentedConstrutors(m_Context));

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }        
    }
}
