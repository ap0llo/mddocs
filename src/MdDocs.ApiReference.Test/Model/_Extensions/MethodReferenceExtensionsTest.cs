using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class MethodReferenceExtensionsTest : TestBase
    {
        private static readonly TypeId s_TestClass_Methods = new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Methods");
        private static readonly TypeId s_TestClass_Operators = new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Operators");
        private static readonly TypeId s_System_String = new SimpleTypeId("System", "String");
        private static readonly TypeId s_System_Int32 = new SimpleTypeId("System", "Int32");

        private MethodReference GetMethodReference(Type type, string methodName)
        {
            return GetTypeDefinition(type)
               .Methods
               .Single(x => x.Name == methodName);
        }


        [Fact]
        public void ToMemberId_returns_the_expected_value_01()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(
                s_TestClass_Methods,
                nameof(TestClass_Methods.TestMethod1)
            );
            var methodReference = GetMethodReference(typeof(TestClass_Methods), expectedMemberId.Name);

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_02()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(
                s_TestClass_Methods,
                nameof(TestClass_Methods.TestMethod2),
                new[] { s_System_String }
            );
            var methodReference = GetMethodReference(typeof(TestClass_Methods), expectedMemberId.Name);

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_03()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(
                s_TestClass_Methods,
                nameof(TestClass_Methods.TestMethod7),
                2,
                new[]
                {
                    new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0),
                    new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1)
                }
            );
            var methodReference = GetMethodReference(typeof(TestClass_Methods), expectedMemberId.Name);

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_04()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(
                s_TestClass_Operators,
                "op_Implicit",
                0,
                new[] { s_TestClass_Operators },
                s_System_String
            );
            var methodReference = GetMethodReference(typeof(TestClass_Operators), expectedMemberId.Name);

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_05()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(
                s_TestClass_Operators,
                "op_Explicit",
                0,
                new[] { s_TestClass_Operators },
                s_System_Int32
            );
            var methodReference = GetMethodReference(typeof(TestClass_Operators), expectedMemberId.Name);

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
