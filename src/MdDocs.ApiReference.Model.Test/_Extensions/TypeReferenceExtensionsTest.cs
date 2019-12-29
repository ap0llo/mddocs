using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Mono.Cecil;
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
        public void ToMemberId_returns_expected_value_for_constructued_types_01()
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
        public void ToMemberId_returns_expected_value_for_constructued_types_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod13))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new GenericTypeInstanceId(
                "System.Collections.Generic",
                "IEnumerable",
                new[]
                {
                    new GenericTypeInstanceId(
                        new NamespaceId("System"),
                        "ArraySegment",
                        new []{ new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0) })
                }
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

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_ref_parameters_01()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_RefParameters))
                .Methods
                .Single(x => x.Name == nameof(TestClass_RefParameters.Method2))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);

        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_ref_parameters_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_RefParameters))
                .Methods
                .Single(x => x.Name == nameof(TestClass_RefParameters.Method3))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ByReferenceTypeId(new ArrayTypeId(new SimpleTypeId("System", "String")));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);

        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_out_parameters()
        {
            // ARRANGE
            var parameter = GetTypeDefinition(typeof(TestClass_RefParameters))
                .Methods
                .Single(x => x.Name == nameof(TestClass_RefParameters.Method1))
                .Parameters
                .Single(); ;

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = parameter.ParameterType.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
            Assert.Equal(ParameterAttributes.Out, parameter.Attributes);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_in_parameters()
        {
            // ARRANGE
            var parameter = GetTypeDefinition(typeof(TestClass_RefParameters))
                .Methods
                .Single(x => x.Name == nameof(TestClass_RefParameters.Method4))
                .Parameters
                .Single(); ;

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = parameter.ParameterType.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
            Assert.Equal(ParameterAttributes.In, parameter.Attributes);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_01()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes.NestedClass1));

            var expectedId = new SimpleTypeId(
                new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes"),
                "NestedClass1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_02()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes.NestedClass1.NestedClass2));

            var expectedId = new SimpleTypeId(
                new SimpleTypeId(
                    new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes"),
                    "NestedClass1"),
                "NestedClass2"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_03()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes.NestedInterface1));

            var expectedId = new SimpleTypeId(
                new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes"),
                "NestedInterface1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_04()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes.NestedClass4<>));

            var expectedId = new GenericTypeId(
                new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes"),
                "NestedClass4",
                1
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_05()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes<>.NestedClass1));

            var expectedId = new SimpleTypeId(
                new GenericTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes", 1),
                "NestedClass1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_06()
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(typeof(TestClass_NestedTypes<>.NestedClass1.NestedClass2<>));

            var expectedId = new GenericTypeId(
                new SimpleTypeId(
                    new GenericTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes", 1),
                    "NestedClass1"),
                "NestedClass2",
                1
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }


        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_01()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_NestedTypes<>))
                .Methods
                .Single(x => x.Name == nameof(TestClass_NestedTypes<string>.Method1))
                .Parameters
                .Single()
                .ParameterType;

            // type: TestClass_NestedTypes.NestedClass4<string> parameter
            var expectedMemberId = new GenericTypeInstanceId(
                new SimpleTypeId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes"),
                "NestedClass4",
                new[] { new SimpleTypeId("System", "String") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_02()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_NestedTypes<>))
                .Methods
                .Single(x => x.Name == nameof(TestClass_NestedTypes<string>.Method2))
                .Parameters
                .Single()
                .ParameterType;

            // type: TestClass_NestedTypes<string>.NestedClass1
            var expectedMemberId = new SimpleTypeId(
                new GenericTypeInstanceId("Grynwald.MdDocs.ApiReference.Test.TestData", "TestClass_NestedTypes",
                    new[] { new SimpleTypeId("System", "String") }),
                "NestedClass1"
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }


        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_03()
        {
            // ARRANGE
            var typeReference = GetTypeDefinition(typeof(TestClass_NestedTypes<>))
                .Methods
                .Single(x => x.Name == nameof(TestClass_NestedTypes<string>.Method3))
                .Parameters
                .Single()
                .ParameterType;

            // type: TestClass_NestedTypes<string>.NestedClass1.NestedClass2<int>
            var expectedMemberId = new GenericTypeInstanceId(
                new SimpleTypeId(
                    new GenericTypeInstanceId(
                        "Grynwald.MdDocs.ApiReference.Test.TestData",
                        "TestClass_NestedTypes",
                        new[] { new SimpleTypeId("System", "String") }),
                    "NestedClass1"),
                "NestedClass2",
                new[] { new SimpleTypeId("System", "Int32") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
