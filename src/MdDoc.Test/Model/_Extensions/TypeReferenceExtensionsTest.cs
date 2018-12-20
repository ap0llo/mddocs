using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MdDoc.Model;
using MdDoc.Test.TestData;
using Xunit;
using Xunit.Sdk;

namespace MdDoc.Test.Model
{
    public class TypeReferenceExtensionsTest : TestBase
    {
        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_01()
        {            
            var typeReference = GetTypeDefinition(typeof(TestClass_Type));
            var expectedMemberId = new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Type");

            var actualMemberId = typeReference.ToMemberId();
            
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_02()
        {
            var typeReference = GetTypeDefinition(typeof(TestClass_GenericType<>));
            var expectedMemberId = new GenericTypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1);

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_constructued_types()
        {
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod6))
                .Parameters
                .Single()
                .ParameterType;
            

            var expectedMemberId = new GenericTypeInstanceId(
                "System.Collections.Generic",
                "IEnumerable",
                new[] { new SimpleTypeId("System", "String") });

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_01()
        {
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod10))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"));
                
            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_02()
        {
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod11))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new ArrayTypeId(new SimpleTypeId("System", "String")));

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_03()
        {
            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod12))
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"), 2);

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_01()
        {

            var typeReference = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod9))
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1);

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_02()
        {

            var typeReference = GetTypeDefinition(typeof(TestClass_GenericType<>))
                .Methods
                .Single(x => x.Name == nameof(TestClass_GenericType<object>.TestMethod1))
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, 0);

            var actualMemberId = typeReference.ToMemberId();

            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
