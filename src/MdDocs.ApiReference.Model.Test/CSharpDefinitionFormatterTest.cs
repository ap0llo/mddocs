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
