using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class MethodPage : OverloadableMemberPage<MethodDocumentation, MethodOverloadDocumentation>
    {
        internal MethodPage(ILinkProvider linkProvider, MethodDocumentation model, ILogger logger)
            : base(linkProvider, model, logger)
        { }


        protected override MdHeading GetPageHeading() =>
           new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Method", 1);
    }
}
