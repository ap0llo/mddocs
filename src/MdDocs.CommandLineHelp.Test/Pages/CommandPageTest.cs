using System;
using System.Collections.Generic;
using System.Text;
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
    public class CommandPageTest
    {
        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var model = new CommandDocumentation("Command1");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_02()
        {
            var model = new CommandDocumentation(
                name: "Command2",
                helpText: "This is the help text of command 2"
            );

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_03()
        {
            var model = new CommandDocumentation(
                name: "Command2",
                options: new[]
                {
                    new OptionDocumentation("parameter1", helpText: "Help text for parameter 1")
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_04()
        {
            var model = new CommandDocumentation(
                name: "Command2",
                options: new[]
                {
                    new OptionDocumentation("parameter1"),
                    new OptionDocumentation("parameter2"),
                    new OptionDocumentation("parameter3", 'x'),
                    new OptionDocumentation(null, 'y'),
                    new OptionDocumentation("parameter4", hidden: true),
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_05()
        {
            var model = new CommandDocumentation(
                name: "Command2",
                options: new[]
                {
                    new OptionDocumentation(null, 'x'),
                    new OptionDocumentation(null, 'y'),
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_06()
        {
            var model = new CommandDocumentation(
                name: "Command2",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "parameter1",
                        helpText: "Description of parameter 1",
                        @default: "some String"),
                    new OptionDocumentation(
                        name: "parameter2",
                        helpText: "Description of parameter 2",
                        @default: 23),
                });

            Approve(model);
        }

        private void Approve(CommandDocumentation model)
        {
            var commandPage = new CommandPage(model);
            var doc = commandPage.GetDocument();

            Assert.NotNull(doc);

            var markdown = doc.ToString();

            var writer = new ApprovalTextWriter(markdown);
            Approvals.Verify(writer, new UnitTestFrameworkNamer(), Approvals.GetReporter());
        }
    }
}
