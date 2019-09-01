using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [UseReporter(typeof(DiffReporter))]
    public class ApplicationPageTest
    {
        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var model = new ApplicationDocumentation(name: "ApplicationName");
            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_02()
        {
            var model = new ApplicationDocumentation(name: "ApplicationName", version: "1.2.3-beta");
            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_03()
        {
            var model = new ApplicationDocumentation(name: "ApplicationName", commands: new[]
            {
                new CommandDocumentation(application: new ApplicationDocumentation("test"), name: "command1", helpText: "Documentation for command 1"),
                new CommandDocumentation(application: new ApplicationDocumentation("test"), name: "command2")
            });

            Approve(model);
        }


        private void Approve(ApplicationDocumentation model)
        {
            var pathProvider = new DefaultPathProvider();
            var documentSet = new DocumentSet<IDocument>();

            // add dummy pages for all commands
            foreach (var command in model.Commands)
            {
                documentSet.Add(pathProvider.GetPath(command), new TextDocument());
            }

            var applicationPage = new ApplicationPage(documentSet, pathProvider, model);
            documentSet.Add(pathProvider.GetPath(model), applicationPage);

            var doc = applicationPage.GetDocument();

            Assert.NotNull(doc);
            var writer = new ApprovalTextWriter(doc.ToString());
            Approvals.Verify(writer, new UnitTestFrameworkNamer(), Approvals.GetReporter());
        }
    }
}
