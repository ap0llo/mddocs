using System;
using System.Linq;
using Grynwald.MdDocs.TestHelpers;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class MethodReferenceExtensionsTest : DynamicCompilationTestBase, IDisposable
    {
        private static readonly TypeId s_TypeId_Class1 = new SimpleTypeId("Grynwald.MdDocs.ApiReference.Model.Test.TestData", "Class1");
        private static readonly TypeId s_TypeId_String = new SimpleTypeId("System", "String");
        private static readonly TypeId s_TypeId_Int32 = new SimpleTypeId("System", "Int32");

        private const string s_SampleSourceCode = @"
            using System;

            namespace Grynwald.MdDocs.ApiReference.Model.Test.TestData
            {
                public class Class1
                {
                    public void Method1()
                    { }

                    public void Method2(string foo)
                    { }

                    public string Method3<T1, T2>(T1 foo, T2 bar) => throw new NotImplementedException();

                    public static implicit operator string(Class1 instance) => throw new NotImplementedException();

                    public static explicit operator int(Class1 instance) => throw new NotImplementedException();
                }
            }
        ";

        private AssemblyDefinition m_Assembly;
        private TypeDefinition m_Class1;

        public MethodReferenceExtensionsTest()
        {
            m_Assembly = Compile(s_SampleSourceCode);
            m_Class1 = m_Assembly.MainModule.Types.Single(x => x.Name == "Class1");
        }

        public void Dispose() => m_Assembly.Dispose();


        [Fact]
        public void ToMemberId_returns_the_expected_value_01()
        {
            // ARRANGE
            var expectedMemberId = new MethodId(s_TypeId_Class1, "Method1");

            var methodReference = m_Class1.Methods.Single(x => x.Name == "Method1");

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
                s_TypeId_Class1,
                "Method2",
                new[] { s_TypeId_String }
            );
            var methodReference = m_Class1.Methods.Single(x => x.Name == "Method2");

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
                s_TypeId_Class1,
                "Method3",
                2,
                new[]
                {
                    new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0),
                    new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1)
                }
            );
            var methodReference = m_Class1.Methods.Single(x => x.Name == "Method3");

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
                s_TypeId_Class1,
                "op_Implicit",
                0,
                new[] { s_TypeId_Class1 },
                s_TypeId_String
            );

            var methodReference = m_Class1.Methods.Single(x => x.Name == "op_Implicit");

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
                s_TypeId_Class1,
                "op_Explicit",
                0,
                new[] { s_TypeId_Class1 },
                s_TypeId_Int32
            );

            var methodReference = m_Class1.Methods.Single(x => x.Name == "op_Explicit");

            // ACT
            var actualMemberId = methodReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
