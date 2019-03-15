using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class EventPage : SimpleMemberPage<EventDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public EventPage(PageFactory pageFactory, string rootOutputPath, EventDocumentation model, ILogger logger)
            : base(pageFactory, rootOutputPath, model, logger)
        {
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Events", $"{Model.Name}.md");
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1);

        // No "Value"section for events
        protected override void AddValueSection(MdContainerBlock block)
        { }
    }
}
