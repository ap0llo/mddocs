using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model.XmlDocs
{
    public class MonoCecilXmlDocExtensionsTest : TestBase
    {        
        [Theory]
        [InlineData("TestClass_Type", "T:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Type")]
        [InlineData("TestClass_GenericType`1", "T:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_GenericType`1")]
        public void GetXmlDocId_returns_the_expected_value_for_types(string typeName, string expectedId)
        {
            // ARRANGE
            var types = m_AssemblyDefinition.Value.MainModule.Types.ToArray();
            var typeDefinition = m_AssemblyDefinition.Value.MainModule.GetTypes().Single(t => t.Name == typeName);
            
            // ACT
            var actualId = typeDefinition.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData(typeof(TestClass_Methods), "TestMethod1", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod1")]
        [InlineData(typeof(TestClass_Methods), "TestMethod2", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod2(System.String)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod3", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod3(System.String)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod4", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod4(System.String,System.String)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod5", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod5``1(``0,System.String)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod6", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod6(System.Collections.Generic.IEnumerable{System.String})")]
        [InlineData(typeof(TestClass_Methods), "TestMethod7", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod7``2(``0,``1)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod8", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod8``2(``0,``1)")]
        [InlineData(typeof(TestClass_Methods), "TestMethod9", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Methods.TestMethod9``2(``1,``0)")]
        [InlineData(typeof(TestClass_GenericType<>), "TestMethod1", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_GenericType`1.TestMethod1(`0)")]
        [InlineData(typeof(TestClass_GenericType<>), "TestMethod2", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_GenericType`1.TestMethod2``1(``0)")]
        [InlineData(typeof(TestClass_GenericType<>), "TestMethod3", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_GenericType`1.TestMethod3``1(``0,`0)")]
        public void GetXmlDocId_returns_the_expected_value_for_methods(Type type, string methodName, string expectedId)
        {
            // ARRANGE            
            var method = GetTypeDefinition(type)
                .Methods
                .Single(m => m.Name == methodName);

            // ACT
            var actualId = method.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData("op_UnaryPlus", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_UnaryPlus(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_UnaryNegation", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_UnaryNegation(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_LogicalNot", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_LogicalNot(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_OnesComplement", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_OnesComplement(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Increment", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Increment(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Decrement", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Decrement(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_True", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_True(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_False", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_False(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Addition", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Addition(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Subtraction", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Subtraction(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Multiply", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Multiply(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Division", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Division(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Modulus", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Modulus(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_BitwiseAnd", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_BitwiseAnd(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_BitwiseOr", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_BitwiseOr(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_ExclusiveOr", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_ExclusiveOr(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_LeftShift", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_LeftShift(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,System.Int32)")]
        [InlineData("op_RightShift", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_RightShift(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,System.Int32)")]
        [InlineData("op_Equality", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Equality(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Inequality", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Inequality(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_LessThan", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_LessThan(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_GreaterThan", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_GreaterThan(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_LessThanOrEqual", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_LessThanOrEqual(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_GreaterThanOrEqual", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_GreaterThanOrEqual(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators,Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)")]
        [InlineData("op_Implicit", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Implicit(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)~System.String")]
        [InlineData("op_Explicit", "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators.op_Explicit(Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Operators)~System.Int32")]
        public void GetXmlDocId_returns_the_expected_value_for_operator_overloads(string methodName, string expectedId)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_Operators))
                .Methods
                .Single(m => m.Name == methodName);

            // ACT
            var actualId = method.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData(0, "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Constructors.#ctor")]
        [InlineData(1, "M:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Constructors.#ctor(System.String)")]
        public void GetXmlDocId_returns_the_expected_value_for_constructors(int parameterCount, string expectedId)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_Constructors))
                .Methods
                .Single(m => m.IsConstructor && m.Parameters.Count == parameterCount);

            // ACT
            var actualId = method.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData("Field1", "F:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Fields.Field1")]
        public void GetXmlDocId_returns_the_expected_value_for_fields(string fieldName, string expectedId)
        {
            // ARRANGE
            var field = GetTypeDefinition(typeof(TestClass_Fields))
                .Fields
                .Single(f => f.Name == fieldName);

            // ACT
            var actualId = field.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData("Property1", "P:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Properties.Property1")]
        [InlineData("Property2", "P:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Properties.Property2")]
        public void GetXmlDocId_returns_the_expected_value_for_properties(string propertyName, string expectedId)
        {
            // ARRANGE
            var property = GetTypeDefinition(typeof(TestClass_Properties))
                .Properties
                .Single(p => p.Name == propertyName);

            // ACT
            var actualId = property.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData(1, "P:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Properties.Item(System.Int32)")]
        [InlineData(2, "P:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Properties.Item(System.Int32,System.Double)")]
        public void GetXmlDocId_returns_the_expected_value_for_indexers(int parameterCount, string expectedId)
        {
            // ARRANGE
            var indexer = GetTypeDefinition(typeof(TestClass_Properties))
                .Properties
                .Single(p => p.Name == "Item" && p.Parameters.Count == parameterCount);

            // ACT
            var actualId = indexer.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }

        [Theory]
        [InlineData("Event1", "E:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Events.Event1")]
        [InlineData("Event2", "E:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Events.Event2")]
        [InlineData("Event3", "E:Grynwald.MdDocs.ApiReference.Test.TestData.TestClass_Events.Event3")]
        public void GetXmlDocId_returns_the_expected_value_for_events(string propertyName, string expectedId)
        {
            // ARRANGE
            var @event = GetTypeDefinition(typeof(TestClass_Events))
                .Events
                .Single(e => e.Name == propertyName);

            // ACT
            var actualId = @event.GetXmlDocId();

            // ASSERT
            Assert.Equal(expectedId, actualId);
        }
    }
}
