using ApprovalTests;
using ApprovalTests.Reporters;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common.Configuration;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [UseReporter(typeof(DiffReporter))]
    public class MultiCommandApplicationPageTest
    {
        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var model = new MultiCommandApplicationDocumentation(name: "ApplicationName");
            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_02()
        {
            var model = new MultiCommandApplicationDocumentation(name: "ApplicationName", version: "1.2.3-beta");
            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_03()
        {
            var model = new MultiCommandApplicationDocumentation(name: "ApplicationName", commands: new[]
            {
                new CommandDocumentation(application: new MultiCommandApplicationDocumentation("test"), name: "command1", helpText: "Documentation for command 1"),
                new CommandDocumentation(application: new MultiCommandApplicationDocumentation("test"), name: "command2")
            });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_04()
        {
            // commands must be ordered by name
            var model = new MultiCommandApplicationDocumentation(name: "ApplicationName", commands: new[]
            {
                new CommandDocumentation(application: new MultiCommandApplicationDocumentation("test"), name: "commandXYZ"),
                new CommandDocumentation(application: new MultiCommandApplicationDocumentation("test"), name: "commandAbc")
            });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_05()
        {
            // commands must be ordered by name
            var model = new MultiCommandApplicationDocumentation(
                name: "ApplicationName",
                usage: new[] { "usage line 1", "usage line 2", "usage line 3" },
                version: "4.5.6");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_06()
        {
            // commands must be ordered by name
            var model = new MultiCommandApplicationDocumentation(
                name: "ApplicationName",
                usage: new[] { "usage line 1", "usage line 2", "usage line 3" },
                version: "4.5.6");

            Approve(model, new CommandLineHelpConfiguration() { IncludeVersion = false });
        }


        private void Approve(MultiCommandApplicationDocumentation model, CommandLineHelpConfiguration? configuration = null)
        {
            var pathProvider = new DefaultCommandLineHelpPathProvider();
            var documentSet = new DocumentSet<IDocument>();

            // add dummy pages for all commands
            foreach (var command in model.Commands)
            {
                documentSet.Add(pathProvider.GetPath(command), new TextDocument());
            }

            configuration ??= DocsConfigurationLoader.GetDefaultConfiguration().CommandLineHelp;

            var applicationPage = new MultiCommandApplicationPage(documentSet, pathProvider, model, configuration);
            documentSet.Add(pathProvider.GetPath(model), applicationPage);

            var doc = applicationPage.GetDocument();

            Assert.NotNull(doc);
            var writer = new ApprovalTextWriter(doc.ToString());
            Approvals.Verify(writer, new ApprovalNamer(), Approvals.GetReporter());
        }
    }
}
