using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class EventPage : SimpleMemberPage<EventDocumentation>
    {        
        public override OutputPath OutputPath { get; }            

        
        public EventPage(PageFactory pageFactory, string rootOutputPath, EventDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "events", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }


        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1);

        protected override void AddValueSection(MdContainerBlock block)
        {            
            // omit value section for events
        }
    }
}
