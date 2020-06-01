using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {
        internal OperatorPage(ILinkProvider linkProvider, OperatorDocumentation model, ILogger logger)
            : base(linkProvider, model, logger)
        { }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
