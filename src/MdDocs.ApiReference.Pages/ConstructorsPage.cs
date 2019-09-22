using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class ConstructorsPage : OverloadableMemberPage<ConstructorDocumentation, ConstructorOverloadDocumentation>
    {
        public override string RelativeOutputPath { get; }


        public ConstructorsPage(ILinkProvider linkProvider, PageFactory pageFactory, ConstructorDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, model, logger)
        {
            RelativeOutputPath = Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Constructors.md");
        }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName} Constructors", 1);

        //No "Returns" subsection for constructors
        protected override void AddReturnsSubSection(MdContainerBlock block, ConstructorOverloadDocumentation overload, int headingLevel)
        { }
    }
}
