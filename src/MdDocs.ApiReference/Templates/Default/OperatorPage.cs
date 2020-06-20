using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public sealed class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {
        internal OperatorPage(ILinkProvider linkProvider, ApiReferenceConfiguration configuration, OperatorDocumentation model, ILogger logger)
            : base(linkProvider, configuration, model, logger)
        { }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
