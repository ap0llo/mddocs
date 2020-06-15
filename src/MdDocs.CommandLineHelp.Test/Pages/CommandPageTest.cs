using ApprovalTests;
using ApprovalTests.Reporters;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model2;
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
        private readonly MultiCommandApplicationDocumentation m_ApplicationDocumentation = new MultiCommandApplicationDocumentation("TestApp", "1.2.3");


        [Fact]
        public void GetDocument_returns_expected_document_01()
        {
            var model = m_ApplicationDocumentation.AddCommand("Command1");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_02()
        {
            var model = m_ApplicationDocumentation.AddCommand("Command2");
            model.Description = "This is the help text of command 2";

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_03()
        {
            var model = m_ApplicationDocumentation.AddCommand("Command2");
            var parameter = model.AddNamedParameter("parameter1", null);
            parameter.Description = "Help text for parameter 1";

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_04()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("Command2")
                    .WithNamedParameter("parameter1", required: true)
                    .WithNamedParameter("parameter2")
                    .WithNamedParameter("parameter3", "x")
                    .WithNamedParameter(shortName: "y");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_05()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("Command2")
                    .WithNamedParameter(shortName: "x")
                    .WithNamedParameter(shortName: "y");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_06()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("Command2")
                    .WithNamedParameter(
                        name: "parameter1",
                        description: "Description of parameter 1",
                        defaultValue: "some String")
                    .WithNamedParameter(
                        name: "parameter2",
                        description: "Description of parameter 2",
                        defaultValue: "23");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_07()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(
                        name: "parameter1",
                        description: "Description of parameter 1",
                        defaultValue: "some String")
                    .WithNamedParameter(
                        name: "parameter2",
                        description: "Description of parameter 2",
                        defaultValue: "23",
                        valuePlaceHolderName: "URI")
                    .WithPositionalParameter(0)
                    .WithPositionalParameter(1, valuePlaceHolderName: "INTEGER")
                    .WithPositionalParameter(2, informationalName: "Value3", valuePlaceHolderName: "STRING");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_08()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(
                        shortName: "a",
                        description: "Description of parameter 1",
                        defaultValue: "some String")
                    .WithNamedParameter(
                        shortName: "b",
                        description: "Description of parameter 2",
                        defaultValue: "23")
                    .WithPositionalParameter(0)
                    .WithPositionalParameter(1, informationalName: "Value2", required: true)
                    .WithPositionalParameter(2, informationalName: "Value3", description: "Help text for value 3");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_09()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(
                        name: "parameter1",
                        description: "Description of parameter 1",
                        defaultValue: "some String",
                        acceptedValues: new[] { "Value1", "Another Value" })
                    .WithPositionalParameter(0, informationalName: "PositionalParameter1", acceptedValues: new[] { "Value1", "Value2" });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_10()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(
                        shortName: "a",
                        acceptedValues: new[] { "Value1", "Another Value" })
                    .WithNamedParameter(
                        shortName: "b",
                        acceptedValues: new[] { "Value1", "Another Value" })
                    .WithPositionalParameter(0, acceptedValues: new[] { "Value1", "Value2" })
                    .WithPositionalParameter(1, acceptedValues: new[] { "Value1", "Value2" });

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_11()
        {
            // parameters must be ordered by name / short name
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(name: "xyz")
                    .WithNamedParameter(shortName: "a");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_document_12()
        {
            // parameters must be ordered by name / short name
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithNamedParameter(name: "xyz")
                    .WithNamedParameter(shortName: "a");

            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();
            configuration.IncludeVersion = false;

            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_returns_expected_document_for_switch_parameters()
        {
            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithSwitchParameter(
                        name: "option1",
                        shortName: "a",
                        description: "Description of parameter 1");

            Approve(model);
        }

        [Fact]
        public void GetDocument_returns_expected_Markdown_for_default_settings()
        {
            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();

            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithSwitchParameter(
                        name: "option1",
                        shortName: "a",
                        description: "Description of parameter 1");

            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_does_not_include_AutoGenerated_notice_if_the_includeAutoGeneratedNotice_setting_is_false()
        {
            var configuration = new ConfigurationProvider().GetDefaultCommandLineHelpConfiguration();
            configuration.IncludeAutoGeneratedNotice = false;

            var model = m_ApplicationDocumentation
                .AddCommand("CommandName")
                    .WithSwitchParameter(
                        name: "option1",
                        shortName: "a",
                        description: "Description of parameter 1");

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
