using System;
using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class ApplicationDocumentationTest
    {
        private ApplicationDocumentation LoadDocumentation()
        {
            var assemblyPath = typeof(Command1Options).Assembly.Location;
            return ApplicationDocumentation.FromAssemblyFile(assemblyPath, NullLogger.Instance);
        }

        [Fact]
        public void Commands_returns_expected_number_of_commands()
        {
            var sut = LoadDocumentation();
            Assert.Equal(3, sut.Commands.Count);
        }

        [Theory]
        [InlineData("command1")]
        [InlineData("command2")]
        [InlineData("command3")]
        public void Expected_command_exists(string name)
        {
            var sut = LoadDocumentation();
            Assert.Contains(sut.Commands, c => c.Name == name);
        }
    }
}
