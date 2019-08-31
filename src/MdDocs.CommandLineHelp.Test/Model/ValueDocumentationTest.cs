using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class ValueDocumentationTest
    {
        private ValueDocumentation LoadDocumentation(string propertyName)
        {
            var assemblyPath = typeof(Command4Options).Assembly.Location;
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);

            var typeDefinition = assemblyDefinition
                .MainModule
                .Types
                .Single(t => t.FullName == typeof(Command4Options).FullName);

            var propertyDefinition = typeDefinition
                .Properties
                .Single(p => p.Name == propertyName);

            return ValueDocumentation.FromPropertyDefinition(propertyDefinition, NullLogger.Instance);
        }

        [Theory]
        [InlineData(nameof(Command4Options.Value1), 0, null, null, false, null)]
        [InlineData(nameof(Command4Options.Value2), 1, "Value2 name", null, false, null)]
        [InlineData(nameof(Command4Options.Value3), 2, null, "Value 3 Help text", false, null)]
        [InlineData(nameof(Command4Options.Value4), 3,null, null, true, null)]
        [InlineData(nameof(Command4Options.Value5), 4,null, null, false, "Value 5 Default")]
        public void Value_has_the_expected_properties(string propertyName, int index, string name, string helpText, bool hidden, object defaultValue)
        {
            var sut = LoadDocumentation(propertyName);

            Assert.Equal(index, sut.Index);
            Assert.Equal(name, sut.Name);
            Assert.Equal(helpText, sut.HelpText);
            Assert.Equal(hidden, sut.Hidden);
            Assert.Equal(defaultValue, sut.Default);
        }
    }
}
