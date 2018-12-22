using System;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class FieldPage : MemberPage<FieldDocumentation>
    {        
        public override OutputPath OutputPath { get; }            

        protected override FieldDocumentation Model { get; }


        public FieldPage(PageFactory pageFactory, string rootOutputPath, FieldDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "fields", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Field", 1)
            );

            AddDeclaringTypeSection(document.Root);

            //TODO: Summary

            document.Root.Add(
                new MdCodeBlock(Model.CSharpDefinition, "csharp")
            );

            //TODO: Field Value

            //TODO: Remarks

            //TODO: Examples

            //TODO: SeeAlso

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }



    }
}
