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
        public void Name_returns_the_CSharp_type_name_for_built_in_types(string propertyName, string expectedTypeName)
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
        public void Name_returns_the_expected_type_name_for_array_types(string propertyName, string expectedTypeName)
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
        public void Name_returns_the_expected_type_name_for_generic_types_with_type_arguments(string propertyName, string expectedTypeName)
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
        public void Name_returns_the_expected_type_name_for_generic_types_with_type_parameters(string propertyName, string expectedTypeName)
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

        [Theory]
        [InlineData(typeof(TestClass_TypeName), "MdDoc.Test.TestData")]
        public void Namespace_returns_the_expected_value(Type type, string expectedNamespace)
        {
            // ARRANGE
            var typeReference = GetTypeReference(type);
                
            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedNamespace, typeName.Namespace);
        }

        [Theory]
        [InlineData(typeof(TestClass_TypeName), "MdDoc.Test.TestData.TestClass_TypeName")]
        public void FullName_returns_the_expected_value(Type type, string expectedFullName)
        {
            // ARRANGE
            var typeReference = GetTypeReference(type);

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedFullName, typeName.FullName);
        }


        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property19), "IEnumerable")]
        [InlineData(nameof(TestClass_TypeName.Property20), "IEnumerable")]
        [InlineData(nameof(TestClass_TypeName.Property21), "Dictionary")]
        public void BaseName_returns_the_expected_type_name_for_generic_types_with_type_arguments(string propertyName, string expectedTypeName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.BaseName);
        }


        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property17))]
        [InlineData(nameof(TestClass_TypeName.Property18))]
        public void IsArray_returns_true_for_Array_types(string propertyName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.True(typeName.IsArray);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property16))]
        public void IsArray_returns_false_for_non_Array_types(string propertyName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.False(typeName.IsArray);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property17), "string")]
        [InlineData(nameof(TestClass_TypeName.Property18), "Stream")]
        public void ElementType_returns_the_expected_type_for_Array_types(string propertyName, string expectedElementName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Equal(expectedElementName, typeName.ElementType.Name);
        }

        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property16))]
        public void ElementType_returns_null_for_non_Array_types(string propertyName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.Null(typeName.ElementType);
        }


        [Theory]
        [InlineData(nameof(TestClass_TypeName.Property16))]
        public void TypeArguments_is_empty_for_non_generic_types(string propertyName)
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_TypeName))
                .Properties
                .Single(p => p.Name == propertyName)
                .PropertyType;

            // ACT
            var typeName = new TypeName(typeReference);

            // ASSERT
            Assert.NotNull(typeName.TypeArguments);
            Assert.Empty(typeName.TypeArguments);
        }

    }
}
