using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class EventPage : SimpleMemberPage<EventDocumentation>
    {

        public override string RelativeOutputPath { get; }


        public EventPage(ILinkProvider linkProvider, PageFactory pageFactory, EventDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, model, logger)
        {
            RelativeOutputPath = Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Events", $"{Model.Name}.md");
        }


        protected override MdHeading GetHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1);

        // No "Value"section for events
        protected override void AddValueSection(MdContainerBlock block)
        { }
    }
}
