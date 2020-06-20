using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public sealed class EventPage : SimpleMemberPage<EventDocumentation>
    {
        internal EventPage(ILinkProvider linkProvider, ApiReferenceConfiguration configuration, EventDocumentation model, ILogger logger)
            : base(linkProvider, configuration, model, logger)
        { }


        protected override MdHeading GetHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1);

        // No "Value"section for events
        protected override void AddValueSection(MdContainerBlock block)
        { }
    }
}
