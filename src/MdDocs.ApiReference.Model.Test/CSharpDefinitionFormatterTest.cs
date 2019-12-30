using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Tests for <see cref="CSharpDefinitionFormatter"/>
    /// </summary>
    public class CSharpDefinitionFormatterTest : TestBase
    {       
        [Theory]
        [InlineData(typeof(TestClass_CSharpDefinition), 1, @"public int this[object parameter] { get; }")]
        [InlineData(typeof(TestClass_CSharpDefinition), 2, @"public int this[object parameter1, Stream parameter2] { get; }")]
        public void GetDefinition_returns_the_expected_definition_for_indexers(Type declaringType, int parameterCount, string expected)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(declaringType)
                .Properties
                .Single(p => p.Name == "Item" && p.Parameters.Count == parameterCount);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(propertyDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Event1), @"public event EventHandler<EventArgs> Event1;")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Event2), @"public static event EventHandler Event2;")]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Event3),
            "[CSharpDefinitionTest1(1)]\r\n" +
            "public static event EventHandler Event3;"
        )]
        public void GetDefinition_returns_the_expected_definition_for_events(Type declaringType, string fieldName, string expected)
        {
            // ARRANGE
            var eventDefinition = GetTypeDefinition(declaringType)
                .Events
                .Single(p => p.Name == fieldName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(eventDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method1), @"public void Method1();")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method2), @"public string Method2();")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method3), @"public string Method3(string param1, Stream param2);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method4), @"public static string Method4(string param1, Stream param2);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method5), @"public static string Method5<TParam>(TParam parameter);")]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method6),
            "[Obsolete]\r\n" +
            "public void Method6();")]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method7),
            "[Obsolete(\"Use another method\")]\r\n" +
            "public void Method7();")]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method8),
            "[CSharpDefinitionTest1(1, Property1 = \"Value\")]\r\n" +
            "public void Method8();"
        )]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method9),
            "[CSharpDefinitionTest2(CSharpDefinitionTestFlagsEnum.Value1 | CSharpDefinitionTestFlagsEnum.Value2)]\r\n" +
            "public void Method9();"
        )]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method10),
            "[CSharpDefinitionTest3(BindingFlags.NonPublic | BindingFlags.CreateInstance)]\r\n" +
            "public void Method10();"
        )]
        [InlineData(
            typeof(TestClass_CSharpDefinition),
            nameof(TestClass_CSharpDefinition.Method11),
            "[CSharpDefinitionTest4(CSharpDefinitionTestEnum.Value2)]\r\n" +
            "public void Method11();"
        )]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method12), "public void Method12(ref int value);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method13), "public void Method13(out string value);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method14), "public void Method14(object parameter1, out string value);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method15), "public void Method15(out string[] value);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method16), "public void Method16(in string value);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method17), "public void Method17(string stringParameter = \"default\");")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method18), "public void Method18(string stringParameter = null, int intParameter = 23);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method19), "public void Method19(CSharpDefinitionTestEnum parameter = CSharpDefinitionTestEnum.Value1);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method20), "public void Method20([Optional]string stringParameter);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method21), "public void Method21(string stringParameter = \"default\");")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method22), "public void Method22([CSharpDefinitionTest5]string parameter1);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method23), "public void Method23([CSharpDefinitionTest4(CSharpDefinitionTestEnum.Value2)][CSharpDefinitionTest5]string parameter1);")]
        [InlineData(typeof(TestClass_CSharpDefinition), nameof(TestClass_CSharpDefinition.Method24), "public void Method24(params string[] parameters);")]
        public void GetDefinition_returns_the_expected_definition_for_methods(Type declaringType, string methodName, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(declaringType)
                .Methods
                .Single(p => p.Name == methodName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(nameof(TestClass_CSharpDefinition_ExtensionMethods.Method1), @"public static void Method1(this string param);")]
        public void GetDefinition_returns_the_expected_definition_for_extension_methods(string methodName, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition_ExtensionMethods))
                .Methods
                .Single(p => p.Name == methodName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(TestClass_CSharpDefinition), 0, false, @"public TestClass_CSharpDefinition();")]
        [InlineData(typeof(TestClass_CSharpDefinition), 1, false, @"public TestClass_CSharpDefinition(string parameter);")]
        [InlineData(typeof(TestClass_CSharpDefinition), 0, true, @"static TestClass_CSharpDefinition();")]
        [InlineData(typeof(CSharpDefinitionTest_GenericClass<>), 0, false, @"public CSharpDefinitionTest_GenericClass();")]
        public void GetDefinition_returns_the_expected_definition_for_constructrs(Type type, int paramterCount, bool isStatic, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(type)
                .Methods
                .Single(p => p.IsStatic == isStatic && p.IsConstructor && p.Parameters.Count == paramterCount);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("op_UnaryPlus", @"public static TestClass_CSharpDefinition operator +(TestClass_CSharpDefinition other);")]
        [InlineData("op_Addition", @"public static TestClass_CSharpDefinition operator +(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_UnaryNegation", @"public static TestClass_CSharpDefinition operator -(TestClass_CSharpDefinition other);")]
        [InlineData("op_Subtraction", @"public static TestClass_CSharpDefinition operator -(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Multiply", @"public static TestClass_CSharpDefinition operator *(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Division", @"public static TestClass_CSharpDefinition operator /(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Modulus", @"public static TestClass_CSharpDefinition operator %(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_BitwiseAnd", @"public static TestClass_CSharpDefinition operator &(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_BitwiseOr", @"public static TestClass_CSharpDefinition operator |(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_LogicalNot", @"public static TestClass_CSharpDefinition operator !(TestClass_CSharpDefinition left);")]
        [InlineData("op_OnesComplement", @"public static TestClass_CSharpDefinition operator ~(TestClass_CSharpDefinition left);")]
        [InlineData("op_Increment", @"public static TestClass_CSharpDefinition operator ++(TestClass_CSharpDefinition left);")]
        [InlineData("op_Decrement", @"public static TestClass_CSharpDefinition operator --(TestClass_CSharpDefinition left);")]
        [InlineData("op_True", @"public static bool operator true(TestClass_CSharpDefinition left);")]
        [InlineData("op_False", @"public static bool operator false(TestClass_CSharpDefinition left);")]
        [InlineData("op_LeftShift", @"public static TestClass_CSharpDefinition operator <<(TestClass_CSharpDefinition left, int right);")]
        [InlineData("op_RightShift", @"public static TestClass_CSharpDefinition operator >>(TestClass_CSharpDefinition left, int right);")]
        [InlineData("op_ExclusiveOr", @"public static TestClass_CSharpDefinition operator ^(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Equality", @"public static bool operator ==(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Inequality", @"public static bool operator !=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_LessThan", @"public static bool operator <(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_GreaterThan", @"public static bool operator >(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_LessThanOrEqual", @"public static bool operator <=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_GreaterThanOrEqual", @"public static bool operator >=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right);")]
        [InlineData("op_Implicit", @"public static implicit operator string(TestClass_CSharpDefinition left);")]
        [InlineData("op_Explicit", @"public static explicit operator int(TestClass_CSharpDefinition left);")]
        public void GetDefinition_returns_the_expected_definition_for_operators(string methodName, string expected)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(typeof(TestClass_CSharpDefinition))
                .Methods
                .Single(p => p.Name == methodName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(fieldDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(nameof(TestClass_CSharpDefinition), "public class TestClass_CSharpDefinition")]
        [InlineData(nameof(CSharpDefinitionTest1Attribute), "public class CSharpDefinitionTest1Attribute : Attribute")]
        [InlineData(nameof(CSharpDefinitionTest_StaticClass), "public static class CSharpDefinitionTest_StaticClass")]
        [InlineData(nameof(CSharpDefinitionTest_AbstractClass), "public abstract class CSharpDefinitionTest_AbstractClass")]
        [InlineData(nameof(CSharpDefinitionTest_SealedClass), "public sealed class CSharpDefinitionTest_SealedClass")]
        [InlineData("CSharpDefinitionTest_GenericClass`1", "public class CSharpDefinitionTest_GenericClass<TParam>")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithAttributes),
            "[CSharpDefinitionTest1(1)]\r\n" +
            "public sealed class CSharpDefinitionTest_ClassWithAttributes"
        )]
        [InlineData(nameof(CSharpDefinitionTest_ExtensionClass), "public static class CSharpDefinitionTest_ExtensionClass")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithInterfaceImplementation), "public class CSharpDefinitionTest_ClassWithInterfaceImplementation : IDisposable")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithMultipleInterfaceImplementations), "public class CSharpDefinitionTest_ClassWithMultipleInterfaceImplementations : IDisposable, IEnumerable")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithbaseTypeAndInterfaceImplementations), "public class CSharpDefinitionTest_ClassWithbaseTypeAndInterfaceImplementations : CSharpDefinitionTest_AbstractClass, IDisposable, IEnumerable")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithBaseType), "public class CSharpDefinitionTest_ClassWithBaseType : CSharpDefinitionTest_AbstractClass")]
        [InlineData(nameof(CSharpDefinitionTest_Interface), "public interface CSharpDefinitionTest_Interface")]
        [InlineData(nameof(CSharpDefinitionTest_InterfaceWithMultipleInterfaceImplementations), "public interface CSharpDefinitionTest_InterfaceWithMultipleInterfaceImplementations : IDisposable, IEnumerable")]
        [InlineData(nameof(CSharpDefinitionTest_Struct), "public struct CSharpDefinitionTest_Struct")]
        [InlineData(nameof(CSharpDefinitionTest_StructWithMultipleInterfaceImplementations), "public struct CSharpDefinitionTest_StructWithMultipleInterfaceImplementations : IDisposable, IEnumerable")]
        [InlineData(nameof(CSharpDefinitionTest_ReadOnlyStruct), "public readonly struct CSharpDefinitionTest_ReadOnlyStruct")]
        [InlineData(
            nameof(CSharpDefinitionTestFlagsEnum),
            "[Flags]\r\n" +
            "public enum CSharpDefinitionTestFlagsEnum : short\r\n" +
            "{\r\n" +
            "    Value1 = 0x1,\r\n" +
            "    Value2 = 0x2,\r\n" +
            "    Value3 = 0x4\r\n" +
            "}\r\n"
        )]
        [InlineData(
            nameof(CSharpDefinitionTestFlagsEnum2),
            "[Flags]\r\n" +
            "public enum CSharpDefinitionTestFlagsEnum2 : short\r\n" +
            "{\r\n" +
            "    Value1 = 0x1,\r\n" +
            "    Value2 = 0x2,\r\n" +
            "    Value3 = 0x4,\r\n" +
            "    All = 0x7\r\n" +
            "}\r\n"
        )]
        [InlineData(
            nameof(CSharpDefinitionTestEnum),
            "public enum CSharpDefinitionTestEnum\r\n" +
            "{\r\n" +
            "    Value1 = 1,\r\n" +
            "    Value2 = 2,\r\n" +
            "    Value3 = 3\r\n" +
            "}\r\n"
        )]
        [InlineData("CSharpDefinitionTest_GenericInterface_Contravariant`1", "public interface CSharpDefinitionTest_GenericInterface_Contravariant<in TParam>")]
        [InlineData("CSharpDefinitionTest_GenericInterface_Covariant`1", "public interface CSharpDefinitionTest_GenericInterface_Covariant<out TParam>")]
        [InlineData("CSharpDefinitionTest_GenericClass2`2", "public class CSharpDefinitionTest_GenericClass2<TParam1, TParam2>")]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithAttribute2),
            "[CSharpDefinitionTest6(CSharpDefinitionTestFlagsEnum2.All)]\r\n" +
            "public class CSharpDefinitionTest_ClassWithAttribute2"
        )]
        [InlineData(nameof(CSharpDefinitionTest_ClassWithAttribute3),
            "[CSharpDefinitionTest6(CSharpDefinitionTestFlagsEnum2.Value1 | CSharpDefinitionTestFlagsEnum2.Value2)]\r\n" +
            "public class CSharpDefinitionTest_ClassWithAttribute3"
        )]
        public void GetDefinition_returns_the_expected_definition_for_types(string typeName, string expected)
        {
            // ARRANGE
            var typeDefinition = m_AssemblyDefinition.Value
                .MainModule
                .Types
                .Single(p => p.Name == typeName);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(typeDefinition);

            // ASSERT
            Assert.Equal(expected, actual);
        }


        
    }
}
