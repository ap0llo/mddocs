using ApprovalTests;
using ApprovalTests.Reporters;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Pages;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    /// <summary>
    /// Tests for <see cref="CommandPage"/>
    /// </summary>
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [UseReporter(typeof(DiffReporter))]
    public class CommandPageTest
    {
        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var model = new CommandDocumentation(new TestAppDocumentation(), "Command1");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_02()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "Command2",
                helpText: "This is the help text of command 2"
            );

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_03()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
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
                application: new TestAppDocumentation(),
                name: "Command2",
                options: new[]
                {
                    new OptionDocumentation("parameter1", required: true),
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
                application: new TestAppDocumentation(),
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
                application: new TestAppDocumentation(),
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

        [Fact]
        public void GetDocument_returns_expected_document_07()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "parameter1",
                        helpText: "Description of parameter 1",
                        @default: "some String"),
                    new OptionDocumentation(
                        name: "parameter2",
                        helpText: "Description of parameter 2",
                        @default: 23,
                        metaValue: "URI")
                },
                values: new[]{
                    new ValueDocumentation(0),
                    new ValueDocumentation(1, metaValue: "INTEGER"),
                    new ValueDocumentation(2, name: "Value3", metaValue: "STRING"),
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_08()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        shortName: 'a',
                        helpText: "Description of parameter 1",
                        @default: "some String"),
                    new OptionDocumentation(
                        shortName: 'b',
                        helpText: "Description of parameter 2",
                        @default: 23)
                },
                values: new[]{
                    new ValueDocumentation(0),
                    new ValueDocumentation(1, name: "Value2", required: true),
                    new ValueDocumentation(2, name: "Value3", helpText: "Help text for value 3"),
                    new ValueDocumentation(3, name: "Value4", hidden: true),
                });

            Approve(model);
        }


        [Fact]
        public void GetDocument_returns_expected_document_09()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "paramter1",
                        helpText: "Description of parameter 1",
                        @default: "some String",
                        acceptedValues: new[] { "Value1", "Another Value"})
                },
                values: new[]{
                    new ValueDocumentation(0, name: "PositionalParameter1", acceptedValues: new[] { "Value1", "Value2"})
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_10()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        shortName: 'a',
                        acceptedValues: new[] { "Value1", "Another Value"}),
                    new OptionDocumentation(
                        shortName: 'b',
                        acceptedValues: new[] { "Value1", "Another Value"})
                },
                values: new[]{
                    new ValueDocumentation(0, acceptedValues: new[] { "Value1", "Value2"}),
                    new ValueDocumentation(1, acceptedValues: new[] { "Value1", "Value2"})
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_11()
        {
            // parameters must be ordered by name / short name
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(name: "xyz"),
                    new OptionDocumentation(shortName: 'a')
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_12()
        {
            // parameters must be ordered by name / short name
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(name: "xyz"),
                    new OptionDocumentation(shortName: 'a')
                });

            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();
            configuration.IncludeVersion = false;

            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_returns_expected_document_for_switch_parameters()
        {
            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "option1",
                        shortName: 'a',
                        helpText: "Description of parameter 1",
                        @default: false,
                        required: false,
                        isSwitchParameter : true),
                });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_Markdown_for_default_settings()
        {
            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();

            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "option1",
                        shortName: 'a',
                        helpText: "Description of parameter 1",
                        @default: false,
                        required: false,
                        isSwitchParameter : true),
                });

            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_does_not_include_AutoGenerated_notice_if_the_includeAutoGeneratedNotice_setting_is_false()
        {
            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();
            configuration.IncludeAutoGeneratedNotice = false;

            var model = new CommandDocumentation(
                application: new TestAppDocumentation(),
                name: "CommandName",
                options: new[]
                {
                    new OptionDocumentation(
                        name: "option1",
                        shortName: 'a',
                        helpText: "Description of parameter 1",
                        @default: false,
                        required: false,
                        isSwitchParameter : true),
                });

            Approve(model, configuration);
        }


        private void Approve(CommandDocumentation model, CommandLineHelpConfiguration? configuration = null)
        {
            var pathProvider = new DefaultCommandLineHelpPathProvider();
            var documentSet = new DocumentSet<IDocument>();

            configuration ??= new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();

            var commandPage = new CommandPage(documentSet, pathProvider, model, configuration);

            // add dummy application page and command page itself to document set
            // because command page will create a link to the application page
            // which would fail otherwise
            documentSet.Add(pathProvider.GetPath(model.Application), new TextDocument());
            documentSet.Add(pathProvider.GetPath(model), commandPage);

            var doc = commandPage.GetDocument();

            Assert.NotNull(doc);

            var markdown = doc.ToString();

            var writer = new ApprovalTextWriter(markdown);
            Approvals.Verify(writer, new ApprovalNamer(), Approvals.GetReporter());
        }
    }
}
