using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;
using Xunit;

namespace MdDoc.Test.Model
{
    public class CSharpDefinitionFormatterTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property1), @"public int Property1 { get; set; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property2), @"public byte Property2 { get; set; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property3), @"public string Property3 { get; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property4), @"public string Property4 { get; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property5), @"public string Property5 { set; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property6), @"public Stream Property6 { get; }")]
        [InlineData(nameof(TestClass_Properties_CSharpDefinition.Property7), @"public IEnumerable<string> Property7 { get; }")]
        public void GetDefinition_returns_the_expected_definition_for_properties(string propertyName, string expected)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(typeof(TestClass_Properties_CSharpDefinition))
                .Properties
                .Single(p => p.Name == propertyName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(propertyDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, @"public int this[object parameter] { get; }")]
        [InlineData(2, @"public int this[object parameter1, Stream parameter2] { get; }")]
        public void GetDefinition_returns_the_expected_definition_for_indexers(int parameterCount, string expected)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(typeof(TestClass_Properties_CSharpDefinition))
                .Properties
                .Single(p => p.Name == "Item" && p.Parameters.Count == parameterCount);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(propertyDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

    }
}
