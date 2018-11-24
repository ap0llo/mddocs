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

        
        public override OutputPath OutputPath => 
            new OutputPath(Path.Combine(GetTypeDir(m_Model.TypeDocumentation.Definition), $"{m_Model.TypeDocumentation.Name}-constructors.md"));

        protected override TypeReference DeclaringType => m_Model.Definitions.First().DeclaringType;

        protected override IDocumentation Model => m_Model;


        
        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, MethodDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Definitions.First().DeclaringType.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, m_Model.Definitions.First().DeclaringType.GetDocumentedConstrutors());

            AddDetailSections(document.Root, m_Model.Definitions.First().DeclaringType.GetDocumentedConstrutors());

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }

        
    }
}
