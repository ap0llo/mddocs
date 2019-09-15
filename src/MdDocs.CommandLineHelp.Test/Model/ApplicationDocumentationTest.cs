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
            Assert.Equal(4, sut.Commands.Count);
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

        [Fact]
        public void Name_returns_expected_value()
        {
            var sut = LoadDocumentation();
            Assert.Equal("TestDataAssemblyTitle", sut.Name);
        }

        [Fact]
        public void Usage_returns_expected_values()
        {
            var sut = LoadDocumentation();

            Assert.NotNull(sut.Usage);
            Assert.Equal(2, sut.Usage.Count);
            Assert.Equal("AssemblyUsage Line 1", sut.Usage.First());
            Assert.Equal("AssemblyUsage Line 2", sut.Usage.Last());
        }
    }
}
