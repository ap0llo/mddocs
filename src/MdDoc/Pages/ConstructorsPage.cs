using System;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class ConstructorsPage : OverloadableMemberPage<ConstructorDocumentation, ConstructorOverloadDocumentation>
    {                
        public override OutputPath OutputPath { get; }
        
        protected override ConstructorDocumentation Model { get; }

        
        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), $"{Model.TypeDocumentation.TypeId.Name}-constructors.md"));
        }

        
        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName} Constructors", 1);
    }
}
