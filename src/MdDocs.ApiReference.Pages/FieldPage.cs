using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class FieldPage : SimpleMemberPage<FieldDocumentation>
    {
        internal FieldPage(ILinkProvider linkProvider, FieldDocumentation model, ILogger logger)
            : base(linkProvider, model, logger)
        { }


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
