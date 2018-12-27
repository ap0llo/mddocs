using System;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Use a different layout if the method is not overloaded
    class MethodPage : OverloadableMemberPage<MethodDocumentation, MethodOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }
            
        protected override MethodDocumentation Model { get; }


        public MethodPage(PageFactory pageFactory, string rootOutputPath, MethodDocumentation model) 
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));   
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Methods", $"{Model.Name}.md");
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Method", 1);        
    }
}
