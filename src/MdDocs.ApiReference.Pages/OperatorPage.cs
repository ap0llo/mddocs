using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class OperatorPage : OverloadableMemberPage<OperatorDocumentation, OperatorOverloadDocumentation>
    {

        public override string RelativeOutputPath { get; }



        public OperatorPage(ILinkProvider linkProvider, PageFactory pageFactory, OperatorDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, model, logger)
        {
            RelativeOutputPath = Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Operators", $"{Model.Kind}.md");
        }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1);
    }
}
