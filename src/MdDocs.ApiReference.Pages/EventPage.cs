using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    class EventPage : SimpleMemberPage<EventDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public EventPage(PageFactory pageFactory, string rootOutputPath, EventDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Events", $"{Model.Name}.md");
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {
            // omit value section for events
        }
    }
}
