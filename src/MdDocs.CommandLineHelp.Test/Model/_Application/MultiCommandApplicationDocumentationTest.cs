using System.Linq;
using System.Reflection;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Grynwald.MdDocs.CommandLineHelp.TestData.SingleCommandApp;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class MultiCommandApplicationDocumentationTest
    {
        private MultiCommandApplicationDocumentation LoadDocumentation(Assembly? assembly = null)
        {
            assembly ??= typeof(Command1Options).Assembly;

            using (var definition = AssemblyReader.ReadFile(assembly.Location, NullLogger.Instance))
            {
                return MultiCommandApplicationDocumentation.FromAssemblyDefinition(definition, NullLogger.Instance);
            }
        }


        [Fact]
        public void Commands_returns_expected_number_of_commands()
        {
            var sut = LoadDocumentation();
            Assert.Equal(4, sut.Commands.Count);
        }

        [Fact]
        public void Abstract_types_are_ignored()
        {
            var sut = LoadDocumentation();
            Assert.DoesNotContain(sut.Commands, c => c.Name == "command5");
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

        [Fact]
        public void Usage_is_empty_for_assemblies_without_usage_attribute()
        {
            var sut = LoadDocumentation(typeof(Options).Assembly);

            Assert.NotNull(sut.Usage);
            Assert.Empty(sut.Usage);
        }
    }
}
