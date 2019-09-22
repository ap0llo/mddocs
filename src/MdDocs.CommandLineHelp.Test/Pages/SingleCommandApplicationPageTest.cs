using ApprovalTests;
using ApprovalTests.Reporters;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [UseReporter(typeof(DiffReporter))]
    public class SingleCommandApplicationPageTest
    {
        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var parameters = new UnnamedCommandDocumentation(
                application: new TestAppDocumentation(),
                options: new[]
                {
                    new OptionDocumentation("parameter1", required: true),
                    new OptionDocumentation("parameter2", 'x', required: true),
                    new OptionDocumentation(null, 'y', required: true),
                },
                values: new[]
                {
                    new ValueDocumentation(0),
                    new ValueDocumentation(1, name: "Value2", required: true),
                    new ValueDocumentation(2, name: "Value3", helpText: "Help text for value 3"),
                    new ValueDocumentation(3, name: "Value4", hidden: true),
                });

            var application = new SingleCommandApplicationDocumentation(name: "ApplicationName", parameters, "1.2.3", new[] { "Usage line 1", "Usage line2" });
            Approve(application);
        }


        private void Approve(SingleCommandApplicationDocumentation model)
        {
            var pathProvider = new DefaultCommandLineHelpPathProvider();
            var documentSet = new DocumentSet<IDocument>();

            var applicationPage = new SingleCommandApplicationPage(documentSet, pathProvider, model);
            documentSet.Add(pathProvider.GetPath(model), applicationPage);

            var doc = applicationPage.GetDocument();

            Assert.NotNull(doc);
            var writer = new ApprovalTextWriter(doc.ToString());
            Approvals.Verify(writer, new ApprovalNamer(), Approvals.GetReporter());
        }
    }
}
