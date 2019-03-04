using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class TypeIdTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property1), "int")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property2), "byte")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property3), "sbyte")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property4), "char")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property5), "decimal")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property6), "double")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property7), "float")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property8), "bool")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property9), "uint")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property10), "long")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property11), "ulong")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property12), "object")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property13), "short")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property14), "ushort")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property15), "string")]
        public void DisplayName_returns_the_CSharp_type_name_for_built_in_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeIdDisplayName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.DisplayName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property17), "string[]")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property18), "Stream[]")]
        public void DisplayName_returns_the_expected_type_name_for_array_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeIdDisplayName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property19), "IEnumerable<string>")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property20), "IEnumerable<Stream>")]
        [InlineData(nameof(TestClass_TypeIdDisplayName.Property21), "Dictionary<string, Stream>")]
        public void DisplayName_returns_the_expected_type_name_for_generic_types_with_type_arguments(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeIdDisplayName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeIdDisplayName<object, object>.Method1), "IEnumerable<T1>")]
        [InlineData(nameof(TestClass_TypeIdDisplayName<object, object>.Method2), "IEnumerable<T2>")]
        [InlineData(nameof(TestClass_TypeIdDisplayName<object, object>.Method3), "Dictionary<T1, T2>")]
        [InlineData(nameof(TestClass_TypeIdDisplayName<object, object>.Method4), "Dictionary<TKey, TValue>")]
        public void DisplayName_returns_the_expected_type_name_for_generic_types_with_type_parameters(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeIdDisplayName<,>))
                .Methods
                .Single(p => p.Name == propertyName)
                .ReturnType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }
    }
}
