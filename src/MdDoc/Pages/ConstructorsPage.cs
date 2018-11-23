using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class ConstructorsPage : MemberPage
    {
        private readonly MethodDocumentation m_Model;

        
        protected override OutputPath OutputPath => m_PathProvider.GetConstructorsOutputPath(m_Model.Definitions.First().DeclaringType);

        protected override TypeReference DeclaringType => m_Model.Definitions.First().DeclaringType;

        
        public ConstructorsPage(PageFactory pageFactory, DocumentationContext context, PathProvider pathProvider, MethodDocumentation model)
            : base(pageFactory, context, pathProvider)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Definitions.First().DeclaringType.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, m_Model.Definitions.First().DeclaringType.GetDocumentedConstrutors(m_Context));

            AddDetailSections(document.Root, m_Model.Definitions.First().DeclaringType.GetDocumentedConstrutors(m_Context));

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }        
    }
}
