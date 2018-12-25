using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class SimpleMemberPage<TModel> : MemberPage<TModel> where TModel : SimpleMemberDocumentation
    {
        protected override TModel Model { get; }

        public SimpleMemberPage(PageFactory pageFactory, string rootOutputPath, TModel model) : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new System.ArgumentNullException(nameof(model));
        }


        public override void Save()
        {
            var document = Document(
                GetHeading()
            );

            AddDeclaringTypeSection(document.Root);

            AddDefinitionSection(document.Root);

            AddValueSection(document.Root);

            //TODO: Remarks

            //TODO: Examples

            //TODO: SeeAlso

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        protected void AddDefinitionSection(MdContainerBlock block)
        {
            if(Model.Summary != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Summary, this));
            }

            block.Add(
                CodeBlock(Model.CSharpDefinition, "csharp")
            );
        }


        protected abstract MdHeading GetHeading();
        
        protected abstract void AddValueSection(MdContainerBlock block);
    }
}
