using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MdDocs.CommandLineHelp.Test.Model
{
    public class ApplicationDocumentationTest
    {
        [Fact]
        public void Commands_returns_expected_commands()
        {
            var assemblyPath = typeof(Command1Options).Assembly.Location;

            var commandLineDocumentation = ApplicationDocumentation.FromAssemblyFile(assemblyPath, NullLogger.Instance);

            Assert.Single(commandLineDocumentation.Commands);

            var command = commandLineDocumentation.Commands.Single();

            Assert.Equal("command1", command.Name);
            Assert.Equal("Some Help Text", command.HelpText);
        }
    }
}
