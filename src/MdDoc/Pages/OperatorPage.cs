using System;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }
            
        protected override OperatorDocumentation Model { get; }


        public OperatorPage(PageFactory pageFactory, string rootOutputPath, OperatorDocumentation model) 
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));   
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "operators", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Kind}.md"));
        }

        
        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
