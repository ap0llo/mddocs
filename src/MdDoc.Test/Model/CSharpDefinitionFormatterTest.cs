using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;
using Xunit;

namespace MdDoc.Test.Model
{
    public class CSharpDefinitionFormatterTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_CSharpDefinition.Property1), @"public int Property1 { get; set; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property2), @"public byte Property2 { get; set; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property3), @"public string Property3 { get; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property4), @"public string Property4 { get; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property5), @"public string Property5 { set; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property6), @"public Stream Property6 { get; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property7), @"public IEnumerable<string> Property7 { get; }")]
        [InlineData(nameof(TestClass_CSharpDefinition.Property8), @"public static IEnumerable<string> Property8 { get; }")]
        public void GetDefinition_returns_the_expected_definition_for_properties(string propertyName, string expected)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
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
            var propertyDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
                .Properties
                .Single(p => p.Name == "Item" && p.Parameters.Count == parameterCount);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(propertyDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(nameof(TestClass_CSharpDefinition.Field1), @"public string Field1;")]
        [InlineData(nameof(TestClass_CSharpDefinition.Field2), @"public static string Field2;")]
        [InlineData(nameof(TestClass_CSharpDefinition.Field3), @"public const string Field3;")]
        [InlineData(nameof(TestClass_CSharpDefinition.Field4), @"public static readonly int Field4;")]
        public void GetDefinition_returns_the_expected_definition_for_fields(string fieldName, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
                .Fields
                .Single(p => p.Name == fieldName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(nameof(TestClass_CSharpDefinition.Method1), @"public void Method1();")]
        [InlineData(nameof(TestClass_CSharpDefinition.Method2), @"public string Method2();")]
        [InlineData(nameof(TestClass_CSharpDefinition.Method3), @"public string Method3(string param1, Stream param2);")]
        [InlineData(nameof(TestClass_CSharpDefinition.Method4), @"public static string Method4(string param1, Stream param2);")]
        [InlineData(nameof(TestClass_CSharpDefinition.Method5), @"public static string Method5<TParam>(TParam parameter);")]
        public void GetDefinition_returns_the_expected_definition_for_methods(string methodName, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
                .Methods
                .Single(p => p.Name == methodName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }



        [Theory]
        [InlineData(0, false, @"public TestClass_CSharpDefinition();")]
        [InlineData(1, false, @"public TestClass_CSharpDefinition(string parameter);")]
        [InlineData(0, true, @"static TestClass_CSharpDefinition();")]
        public void GetDefinition_returns_the_expected_definition_for_constructrs(int paramterCount, bool isStatic, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
                .Methods
                .Single(p => p.IsStatic == isStatic && p.IsConstructor && p.Parameters.Count == paramterCount);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

    }
}
