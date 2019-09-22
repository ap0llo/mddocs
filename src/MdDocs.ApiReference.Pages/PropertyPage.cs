using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class PropertyPage : SimpleMemberPage<PropertyDocumentation>
    {
        internal PropertyPage(ILinkProvider linkProvider, PageFactory pageFactory, PropertyDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, model, logger)
        { }


        protected override MdHeading GetHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Property", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            block.Add(new MdHeading("Property Value", 2));
            block.Add(
                GetMdParagraph(Model.Type)
            );

            if (Model.Value != null)
            {
                block.Add(ConvertToBlock(Model.Value));
            }

            // After the value section, add the Exceptions section
            AddExceptionsSection(block);
        }

        private void AddExceptionsSection(MdContainerBlock block)
        {
            if (Model.Exceptions.Count == 0)
                return;

            block.Add(new MdHeading("Exceptions", 2));

            foreach (var exception in Model.Exceptions)
            {
                block.Add(
                    GetMdParagraph(exception.Type),
                    ConvertToBlock(exception.Text)
                );
            }
        }
    }
}
