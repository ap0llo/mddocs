using System;
using System.Linq;
using MdDoc.Model;
using MdDoc.Model.XmlDocs;
using MdDoc.Test.TestData;
using Xunit;

namespace MdDoc.Test.Model
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
        [InlineData("op_Implicit", OperatorKind.ImplicitConversion)]
        [InlineData("op_Explicit", OperatorKind.ExplicitConversion)]
        public void Kind_returns_the_expected_value(string methodName, OperatorKind expectedKind)
        {
            // ARRANGE
            var methodDefinition = GetTypeDefinition(typeof(TestClass_Operators))
                .Methods
                .Single(m => m.Name == methodName);

            // ACT
            var operatorDocumentation = new OperatorDocumentation(
                GetTypeDocumentation(typeof(TestClass_Operators)),
                new[] { methodDefinition },
                NullXmlDocsProvider.Instance);

            // ASSERT
            Assert.Equal(expectedKind, operatorDocumentation.Kind);
        }

        [Fact]
        public void Constructor_throw_ArgumentException_if_specified_method_is_not_an_operator_overload()
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_Methods))
                .Methods
                .Single(x => x.Name == nameof(TestClass_Methods.TestMethod1));

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => new OperatorDocumentation(
                GetTypeDocumentation(typeof(TestClass_Methods)),
                new[] { method },
                NullXmlDocsProvider.Instance)
            );
        }

        [Fact]
        public void Constructor_throw_ArgumentException_if_overloads_of_different_operators_are_passed_in()
        {
            // ARRANGE
            var methods = GetTypeDefinition(typeof(TestClass_Operators))
                .Methods
                .Where(x => x.GetOperatorKind() == OperatorKind.Subtraction || x.GetOperatorKind() == OperatorKind.Addition);

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => new OperatorDocumentation(
                GetTypeDocumentation(typeof(TestClass_Operators)),
                methods,
                NullXmlDocsProvider.Instance)
            );
        }


        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Operators)).Operators.First();
        }
    }
}
