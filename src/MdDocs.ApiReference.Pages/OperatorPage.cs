using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {
        internal OperatorPage(ILinkProvider linkProvider, PageFactory pageFactory, OperatorDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, model, logger)
        {
        }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
