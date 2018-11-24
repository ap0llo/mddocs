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
    class ConstructorsPage : MemberPage<ConstructorDocumentation>
    {                
        public override OutputPath OutputPath => 
            new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation.Definition), $"{Model.TypeDocumentation.Name}-constructors.md"));


        protected override TypeReference DeclaringType => Model.Definitions.First().DeclaringType;

        protected override ConstructorDocumentation Model { get; }

        
        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.Definitions.First().DeclaringType.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, Model.Definitions.First().DeclaringType.GetDocumentedConstrutors());

            AddDetailSections(document.Root, Model.Definitions.First().DeclaringType.GetDocumentedConstrutors());

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }
    }
}
