using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class ConstructorsPage : OverloadableMemberPage<ConstructorDocumentation, ConstructorOverloadDocumentation>
    {
        public override string RelativeOutputPath { get; }

        public override OutputPath OutputPath { get; }


        public ConstructorsPage(ILinkProvider linkProvider, PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model, ILogger logger)
            : base(linkProvider, pageFactory, rootOutputPath, model, logger)
        {
            RelativeOutputPath = Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Constructors.md");
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Constructors.md");
        }


        protected override MdHeading GetPageHeading() =>
            new MdHeading($"{Model.TypeDocumentation.DisplayName} Constructors", 1);

        //No "Returns" subsection for constructors
        protected override void AddReturnsSubSection(MdContainerBlock block, ConstructorOverloadDocumentation overload, int headingLevel)
        { }
    }
}
