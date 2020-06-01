using System.IO;
using Grynwald.MdDocs.CommandLineHelp.Commands;
using Grynwald.MdDocs.CommandLineHelp.Test.Model;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Commands
{
    public class CommandLineHelpCommandTest : CommandLineDynamicCompilationTestBase
    {
        private readonly ILogger m_Logger = NullLogger.Instance;


        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("does-not-exists.dll")]
        public void Execute_returns_false_if_AssemblyPath_is_invalid(string assemblyPath)
        {
            // ARRANGE
            var configuration = new DocsConfiguration()
            {
                CommandLineHelp = new CommandLineHelpConfiguration()
                {
                    AssemblyPath = assemblyPath,
                    OutputPath = "./some-output-path"
                }
            };

            var sut = new CommandLineHelpCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.False(success);
        }

        [Fact]
        public void Execute_generates_commandlinehelp_output()
        {
            // ARRANGE
            using var temporaryDirectory = new TemporaryDirectory();

            var assemblyPath = Path.Combine(temporaryDirectory, $"myAssembly.dll");
            var xmlDocumentationPath = Path.ChangeExtension(assemblyPath, ".xml");
            var outputPath = Path.Combine(temporaryDirectory, "output");

            CompileToFile(@"
                using CommandLine;

                public class Class1
                {
                    [Option(""option1"", Required = true)]
                    public string Option1 { get; set; }

                    [Option(""option2"", Required = true)]
                    public int Option2 { get; set; }

                    [Option(""option3"", Required = true)]
                    public int Option3 { get; set; }
                }
            ", assemblyPath, xmlDocumentationPath);

            var configuration = new DocsConfiguration()
            {
                CommandLineHelp = new CommandLineHelpConfiguration()
                {
                    AssemblyPath = assemblyPath,
                    OutputPath = outputPath
                }
            };

            var sut = new CommandLineHelpCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.True(success);
            Assert.True(Directory.Exists(outputPath));
            Assert.True(File.Exists(Path.Combine(outputPath, "index.md")));
        }
    }
}
