using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class TypeReferenceExtensionsTest : TestBase
    {
        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_01()
        {            
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Type));
            var expectedMemberId = new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_Type");

            // ACT
            var actualMemberId = typeReference.ToMemberId();
            
            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_GenericType<>));
            var expectedMemberId = new GenericTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_GenericType", 1);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_constructued_types()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod6))
                .Parameters
                .Single()
                .ParameterType;
                    
            var expectedMemberId = new GenericTypeInstanceId(
                "System.Collections.Generic",
                "IEnumerable",
                new[] { new SimpleTypeId("System", "String") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_01()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod10))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod11))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new ArrayTypeId(new SimpleTypeId("System", "String")));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_03()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod12))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"), 2);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_01()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod9))
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_GenericType<>))
                .Methods
                .Single(x => x.Name == nameof(TestClass_GenericType<object>.TestMethod1))
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, 0);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
