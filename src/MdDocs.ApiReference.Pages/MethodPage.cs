using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class MethodPage : OverloadableMemberPage<MethodDocumentation, MethodOverloadDocumentation>
    {
        public override string RelativeOutputPath { get; }

        public override OutputPath OutputPath { get; }


        public MethodPage(PageFactory pageFactory, string rootOutputPath, MethodDocumentation model, ILogger logger)
            : base(pageFactory, rootOutputPath, model, logger)
        {
            RelativeOutputPath = Path.Combine(GetTypeDirRelative(Model.TypeDocumentation), "Methods", $"{Model.Name}.md");
            OutputPath = new OutputPath(GetTypeDir(Model.TypeDocumentation), "Methods", $"{Model.Name}.md");
        }


        protected override MdHeading GetPageHeading() =>
           new MdHeading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Method", 1);
    }
}
