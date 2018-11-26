using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class TypeNameTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property1), "int")]
        [InlineData(nameof(TestClass_TypeName.Property2), "byte")]
        [InlineData(nameof(TestClass_TypeName.Property3), "sbyte")]
        [InlineData(nameof(TestClass_TypeName.Property4), "char")]
        [InlineData(nameof(TestClass_TypeName.Property5), "decimal")]
        [InlineData(nameof(TestClass_TypeName.Property6), "double")]
        [InlineData(nameof(TestClass_TypeName.Property7), "float")]
        [InlineData(nameof(TestClass_TypeName.Property8), "bool")]
        [InlineData(nameof(TestClass_TypeName.Property9), "uint")]
        [InlineData(nameof(TestClass_TypeName.Property10), "long")]
        [InlineData(nameof(TestClass_TypeName.Property11), "ulong")]
        [InlineData(nameof(TestClass_TypeName.Property12), "object")]
        [InlineData(nameof(TestClass_TypeName.Property13), "short")]
        [InlineData(nameof(TestClass_TypeName.Property14), "ushort")]
        [InlineData(nameof(TestClass_TypeName.Property15), "string")]
        public void GetTypeName_returns_the_CSharp_type_name_for_built_in_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.Name);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property16), "Stream")]
        public void GetTypeName_returns_the_type_name_for_non_built_in_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.Name);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property17), "string[]")]
        [InlineData(nameof(TestClass_TypeName.Property18), "Stream[]")]
        public void GetTypeName_returns_the_expected_type_name_for_array_types(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.Name);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property19), "IEnumerable<string>")]
        [InlineData(nameof(TestClass_TypeName.Property20), "IEnumerable<Stream>")]
        [InlineData(nameof(TestClass_TypeName.Property21), "Dictionary<string, Stream>")]
        public void GetTypeName_returns_the_expected_type_name_for_generic_types_with_type_arguments(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.Name);
        }

        [Theory]
        [InlineData("Property1", "IEnumerable<T1>")]
        [InlineData("Property2", "IEnumerable<T2>")]
        [InlineData("Property3", "Dictionary<T1, T2>")]
        public void GetTypeName_returns_the_expected_type_name_for_generic_types_with_type_parameters(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName<,>))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.Name);
        }

    }
}
