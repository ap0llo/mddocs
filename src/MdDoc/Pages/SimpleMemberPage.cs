using System.IO;
using System.Linq;
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

            AddRemarksSection(document.Root);

            //TODO: Examples

            AddSeeAlsoSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        protected virtual void AddDefinitionSection(MdContainerBlock block)
        {
            if(Model.Summary != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Summary, this));
            }

            block.Add(
                CodeBlock(Model.CSharpDefinition, "csharp")
            );
        }

        protected virtual void AddRemarksSection(MdContainerBlock block)
        {
            if (Model.Remarks == null)
                return;

            block.Add(Heading(2, "Remarks"));
            block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Remarks, this));
        }

        protected virtual void AddSeeAlsoSection(MdContainerBlock block)
        {
            if (Model.SeeAlso.Count > 0)
            {
                block.Add(Heading(2, "See Also"));
                block.Add(
                    BulletList(
                        Model.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
                ));
            }
        }


        protected abstract MdHeading GetHeading();
        
        protected abstract void AddValueSection(MdContainerBlock block);
    }
}
