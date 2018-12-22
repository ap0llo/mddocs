using System;
using System.IO;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Use a different layout if there is only a single overloads for this operator
    class OperatorPage : MemberPage<OperatorDocumentation>
    {
        public override OutputPath OutputPath { get; }
            
        protected override OperatorDocumentation Model { get; }


        public OperatorPage(PageFactory pageFactory, string rootOutputPath, OperatorDocumentation model) 
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));   
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "operators", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Kind}.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1)
            );

            AddDeclaringTypeSection(document.Root);            

            //TODO: Summary

            //TODO: Overloads
           
            //TODO: Details section
           
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }
    }
}
