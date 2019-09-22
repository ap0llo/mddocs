using System.Reflection;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData.SingleCommandApp;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class SingleCommandApplicationDocumentationTest
    {
        private SingleCommandApplicationDocumentation LoadDocumentation(Assembly assembly)
        {
            var assemblyDefinition = AssemblyReader.ReadFile(assembly.Location, NullLogger.Instance);
            return SingleCommandApplicationDocumentation.FromAssemblyDefinition(assemblyDefinition, NullLogger.Instance);
        }


        [Fact]
        public void Parameters_is_not_null_when_no_option_classes_can_be_found()
        {
            var sut = LoadDocumentation(Assembly.GetExecutingAssembly());

            Assert.NotNull(sut.Command);
            Assert.Empty(sut.Command.Options);
            Assert.Empty(sut.Command.Values);
            Assert.Empty(sut.Command.Parameters);
        }

        [Theory]
        [InlineData("option1")]
        [InlineData("option2")]
        [InlineData("option4")]
        [InlineData("option5")]
        public void Expected_options_exist(string name)
        {
            var sut = LoadDocumentation(typeof(Options).Assembly);
            Assert.Contains(sut.Command.Options, c => c.Name == name);
        }

        [Theory]
        [InlineData("option3")]
        public void Hidden_options_are_ignored(string name)
        {
            var sut = LoadDocumentation(typeof(Options).Assembly);
            Assert.DoesNotContain(sut.Command.Options, c => c.Name == name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Expected_values_exist(int index)
        {
            var sut = LoadDocumentation(typeof(Options).Assembly);
            Assert.Contains(sut.Command.Values, c => c.Index == index);
        }
    }
}
