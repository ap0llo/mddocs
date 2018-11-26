using MdDoc.Model;
using System;
using System.IO;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Add documentation from XML comments
    class ConstructorsPage : MemberPage<ConstructorDocumentation>
    {                
        public override OutputPath OutputPath { get; }
        
        protected override ConstructorDocumentation Model { get; }

        
        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), $"{Model.TypeDocumentation.Name}-constructors.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.TypeDocumentation.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);
            
            AddOverloadsSection(document.Root, Model.Definitions);

            AddDetailSections(document.Root, Model.Definitions);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }
    }
}
