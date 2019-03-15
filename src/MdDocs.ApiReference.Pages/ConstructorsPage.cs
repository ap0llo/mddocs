using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class ConstructorsPage : OverloadableMemberPage<ConstructorDocumentation, ConstructorOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public ConstructorsPage(PageFactory pageFactory, string rootOutputPath, ConstructorDocumentation model, ILogger logger)
            : base(pageFactory, rootOutputPath, model, logger)
        {
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Constructors.md");
        }


        protected override MdHeading GetPageHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName} Constructors", 1);

        //No "Returns" subsection for constructors
        protected override void AddReturnsSubSection(MdContainerBlock block, ConstructorOverloadDocumentation overload, int headingLevel)
        { }
    }
}
