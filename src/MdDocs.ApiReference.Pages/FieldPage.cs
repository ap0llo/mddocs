using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class FieldPage : SimpleMemberPage<FieldDocumentation>
    {
        public override string RelativeOutputPath { get; }

        public override OutputPath OutputPath { get; }


        public FieldPage(PageFactory pageFactory, string rootOutputPath, FieldDocumentation model, ILogger logger)
            : base(pageFactory, rootOutputPath, model, logger)
        {
            RelativeOutputPath= Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Fields", $"{Model.Name}.md");
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Fields", $"{Model.Name}.md");
        }


        protected override MdHeading GetHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Field", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            block.Add(new MdHeading("Field Value", 2));
            block.Add(GetMdParagraph(Model.Type));

            if (Model.Value != null)
            {
                block.Add(ConvertToBlock(Model.Value));
            }
        }
    }
}
