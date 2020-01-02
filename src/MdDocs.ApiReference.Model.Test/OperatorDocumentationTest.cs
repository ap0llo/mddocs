using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class OperatorDocumentationTest : MemberDocumentationTest
    {
        [Theory]
        [InlineData("op_UnaryPlus", OperatorKind.UnaryPlus)]
        [InlineData("op_UnaryNegation", OperatorKind.UnaryNegation)]
        [InlineData("op_LogicalNot", OperatorKind.LogicalNot)]
        [InlineData("op_OnesComplement", OperatorKind.OnesComplement)]
        [InlineData("op_Increment", OperatorKind.Increment)]
        [InlineData("op_Decrement", OperatorKind.Decrement)]
        [InlineData("op_True", OperatorKind.True)]
        [InlineData("op_False", OperatorKind.False)]
        [InlineData("op_Addition", OperatorKind.Addition)]
        [InlineData("op_Subtraction", OperatorKind.Subtraction)]
        [InlineData("op_Multiply", OperatorKind.Multiply)]
        [InlineData("op_Division", OperatorKind.Division)]
        [InlineData("op_Modulus", OperatorKind.Modulus)]
        [InlineData("op_BitwiseAnd", OperatorKind.BitwiseAnd)]
        [InlineData("op_BitwiseOr", OperatorKind.BitwiseOr)]
        [InlineData("op_ExclusiveOr", OperatorKind.ExclusiveOr)]
        [InlineData("op_LeftShift", OperatorKind.LeftShift)]
        [InlineData("op_RightShift", OperatorKind.RightShift)]
        [InlineData("op_Equality", OperatorKind.Equality)]
        [InlineData("op_Inequality", OperatorKind.Inequality)]
        [InlineData("op_LessThan", OperatorKind.LessThan)]
        [InlineData("op_GreaterThan", OperatorKind.GreaterThan)]
        [InlineData("op_LessThanOrEqual", OperatorKind.LessThanOrEqual)]
        [InlineData("op_GreaterThanOrEqual", OperatorKind.GreaterThanOrEqual)]
        [InlineData("op_Implicit", OperatorKind.Implicit)]
        [InlineData("op_Explicit", OperatorKind.Explicit)]
        public void Kind_returns_the_expected_value(string methodName, OperatorKind expectedKind)
        {
            // ARRANGE
            var cs = @"
                using System;

                public class Class1
                {
                    public static Class1 operator +(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator *(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator /(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator %(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator &(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator |(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator !(Class1 left) => throw new NotImplementedException();

                    public static Class1 operator ~(Class1 left) => throw new NotImplementedException();

                    public static Class1 operator ++(Class1 left) => throw new NotImplementedException();

                    public static Class1 operator --(Class1 left) => throw new NotImplementedException();

                    public static bool operator true(Class1 left) => throw new NotImplementedException();

                    public static bool operator false(Class1 left) => throw new NotImplementedException();

                    public static Class1 operator <<(Class1 left, int right) => throw new NotImplementedException();

                    public static Class1 operator >>(Class1 left, int right) => throw new NotImplementedException();

                    public static Class1 operator ^(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator ==(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator !=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator <(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator >(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator <=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static bool operator >=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static implicit operator string(Class1 left) => throw new NotImplementedException();

                    public static explicit operator int(Class1 left) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var methodDefinition = assembly.MainModule
                .Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(m => m.Name == methodName);

            using var assemblyDocumentaton = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentaton.MainModuleDocumentation.Types.Single();

            // ACT
            var operatorDocumentation = new OperatorDocumentation(
                typeDocumentation,
                new[] { methodDefinition },
                NullXmlDocsProvider.Instance);

            // ASSERT
            Assert.Equal(expectedKind, operatorDocumentation.Kind);
        }

        [Fact]
        public void Constructor_throw_ArgumentException_if_specified_method_is_not_an_operator_overload()
        {
            // ARRANGE
            var cs = @"
                using System;

                public class Class1
                {
                    public static Class1 Method1(Class1 other) => throw new NotImplementedException();

                }
            ";

            using var assembly = Compile(cs);

            var methodDefinition = assembly.MainModule
                .Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(m => m.Name == "Method1");

            using var assemblyDocumentaton = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentaton.MainModuleDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => new OperatorDocumentation(
                typeDocumentation,
                new[] { methodDefinition },
                NullXmlDocsProvider.Instance)
            );
        }

        [Fact]
        public void Constructor_throw_ArgumentException_if_overloads_of_different_operators_are_passed_in()
        {
            // ARRANGE
            var cs = @"
                using System;

                public class Class1
                {
                    public static Class1 operator +(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 left, Class1 right) => throw new NotImplementedException();

                }
            ";

            using var assembly = Compile(cs);

            var methods = assembly.MainModule
                .Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Where(x => x.GetOperatorKind() == OperatorKind.Subtraction || x.GetOperatorKind() == OperatorKind.Addition);

            using var assemblyDocumentaton = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentaton.MainModuleDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => new OperatorDocumentation(
                typeDocumentation,
                methods,
                NullXmlDocsProvider.Instance)
            );
        }


        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Operators.First();
        }
    }
}
