using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class TypeNameFormatterTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_TypeFormatter.Property1), "int")]
        [InlineData(nameof(TestClass_TypeFormatter.Property2), "byte")]
        [InlineData(nameof(TestClass_TypeFormatter.Property3), "sbyte")]
        [InlineData(nameof(TestClass_TypeFormatter.Property4), "char")]
        [InlineData(nameof(TestClass_TypeFormatter.Property5), "decimal")]
        [InlineData(nameof(TestClass_TypeFormatter.Property6), "double")]
        [InlineData(nameof(TestClass_TypeFormatter.Property7), "float")]
        [InlineData(nameof(TestClass_TypeFormatter.Property8), "bool")]
        [InlineData(nameof(TestClass_TypeFormatter.Property9), "uint")]
        [InlineData(nameof(TestClass_TypeFormatter.Property10), "long")]
        [InlineData(nameof(TestClass_TypeFormatter.Property11), "ulong")]
        [InlineData(nameof(TestClass_TypeFormatter.Property12), "object")]
        [InlineData(nameof(TestClass_TypeFormatter.Property13), "short")]
        [InlineData(nameof(TestClass_TypeFormatter.Property14), "ushort")]
        [InlineData(nameof(TestClass_TypeFormatter.Property15), "string")]
        public void GetTypeName_returns_the_CSharp_type_name_for_built_in_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeFormatter))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            var sut = TypeNameFormatter.Instance;

            // ACT
            var acutalTypeName = sut.GetTypeName(typeReference);

            //ASSERT
            Assert.Equal(expectedTypeName, acutalTypeName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeFormatter.Property16), "Stream")]
        public void GetTypeName_returns_the_type_name_for_non_built_in_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeFormatter))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            var sut = TypeNameFormatter.Instance;

            // ACT
            var actualTypeName = sut.GetTypeName(typeReference);

            //ASSERT
            Assert.Equal(expectedTypeName, actualTypeName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeFormatter.Property17), "string[]")]
        [InlineData(nameof(TestClass_TypeFormatter.Property18), "Stream[]")]
        public void GetTypeName_returns_the_expected_type_name_for_array_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeFormatter))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            var sut = TypeNameFormatter.Instance;

            // ACT
            var actualTypeName = sut.GetTypeName(typeReference);

            //ASSERT
            Assert.Equal(expectedTypeName, actualTypeName);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeFormatter.Property19), "IEnumerable<string>")]
        [InlineData(nameof(TestClass_TypeFormatter.Property20), "IEnumerable<Stream>")]
        [InlineData(nameof(TestClass_TypeFormatter.Property21), "Dictionary<string, Stream>")]
        public void GetTypeName_returns_the_expected_type_name_for_generic_types_with_type_arguments(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeFormatter))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            var sut = TypeNameFormatter.Instance;

            // ACT
            var actualTypeName = sut.GetTypeName(typeReference);

            //ASSERT
            Assert.Equal(expectedTypeName, actualTypeName);
        }

        [Theory]
        [InlineData("Property1", "IEnumerable<T1>")]
        [InlineData("Property2", "IEnumerable<T2>")]
        [InlineData("Property3", "Dictionary<T1, T2>")]
        public void GetTypeName_returns_the_expected_type_name_for_generic_types_with_type_parameters(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeFormatter<,>))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            var sut = TypeNameFormatter.Instance;

            // ACT
            var actualTypeName = sut.GetTypeName(typeReference);

            //ASSERT
            Assert.Equal(expectedTypeName, actualTypeName);
        }

    }
}
