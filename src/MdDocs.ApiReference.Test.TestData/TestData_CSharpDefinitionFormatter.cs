using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    public class CSharpDefinitionTest1Attribute : Attribute
    {
        public string Property1 { get; set; }

        public CSharpDefinitionTest1Attribute(int value)
        {
        }
    }

    [Flags]
    public enum CSharpDefinitionTestFlagsEnum : short
    {
        Value1 = 0x01,
        Value2 = 0x01 << 1,
        Value3 = 0x01 << 2
    }

    public class CSharpDefinitionTest2Attribute : Attribute
    {
        public CSharpDefinitionTest2Attribute(CSharpDefinitionTestFlagsEnum value)
        {
        }
    }

    [Flags]
    public enum CSharpDefinitionTestFlagsEnum2 : short
    {
        Value1 = 0x01,
        Value2 = 0x01 << 1,
        Value3 = 0x01 << 2,
        All = Value1 | Value2 | Value3
    }


    public class CSharpDefinitionTest3Attribute : Attribute
    {
        public CSharpDefinitionTest3Attribute(BindingFlags value)
        {
        }
    }

    public class CSharpDefinitionTest4Attribute : Attribute
    {
        public CSharpDefinitionTest4Attribute(CSharpDefinitionTestEnum value)
        {
        }
    }

    public class CSharpDefinitionTest5Attribute : Attribute
    {
    }


    public class CSharpDefinitionTest6Attribute : Attribute
    {
        public CSharpDefinitionTest6Attribute(CSharpDefinitionTestFlagsEnum2 value)
        {
        }
    }

    [CSharpDefinitionTest6(CSharpDefinitionTestFlagsEnum2.All)]
    public class CSharpDefinitionTest_ClassWithAttribute2
    { }

    [CSharpDefinitionTest6(CSharpDefinitionTestFlagsEnum2.Value1 | CSharpDefinitionTestFlagsEnum2.Value2)]
    public class CSharpDefinitionTest_ClassWithAttribute3
    { }

    public enum CSharpDefinitionTestEnum
    {
        Value1 = 1,
        Value2 = 2,
        Value3 = 3,
    }


    public static class CSharpDefinitionTest_StaticClass
    {
    }

    public abstract class CSharpDefinitionTest_AbstractClass
    {
    }

    public sealed class CSharpDefinitionTest_SealedClass
    {
    }

    public class CSharpDefinitionTest_GenericClass<TParam>
    {
    }


    public class CSharpDefinitionTest_GenericClass2<TParam1, TParam2>
    {
    }


    public interface CSharpDefinitionTest_GenericInterface_Contravariant<in TParam>
    {
    }

    public interface CSharpDefinitionTest_GenericInterface_Covariant<out TParam>
    {
    }

    // the C# compiler emits a "Extension" attribute for classes that contain extension methods
    // this attribute should not be included in the generated definition
    public static class CSharpDefinitionTest_ExtensionClass
    {
        public static void Foo(this string str) { }
    }

    [CSharpDefinitionTest1(1)]
    public sealed class CSharpDefinitionTest_ClassWithAttributes
    {
    }

    public class CSharpDefinitionTest_ClassWithInterfaceImplementation : IDisposable
    {
        public void Dispose() => throw new NotImplementedException();
    }

    public class CSharpDefinitionTest_ClassWithMultipleInterfaceImplementations : IDisposable, IEnumerable
    {
        public void Dispose() => throw new NotImplementedException();

        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    public class CSharpDefinitionTest_ClassWithbaseTypeAndInterfaceImplementations : CSharpDefinitionTest_AbstractClass, IDisposable, IEnumerable
    {
        public void Dispose() => throw new NotImplementedException();

        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    public class CSharpDefinitionTest_ClassWithBaseType : CSharpDefinitionTest_AbstractClass
    {
    }

    public interface CSharpDefinitionTest_Interface
    { }

    public interface CSharpDefinitionTest_InterfaceWithMultipleInterfaceImplementations : IDisposable, IEnumerable
    { }

    public struct CSharpDefinitionTest_Struct
    { }

    public struct CSharpDefinitionTest_StructWithMultipleInterfaceImplementations : IDisposable, IEnumerable
    {
        public void Dispose() => throw new NotImplementedException();

        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    public readonly struct CSharpDefinitionTest_ReadOnlyStruct
    { }



    public class TestClass_CSharpDefinition
    {
        public string Field1;

        public static string Field2;

        public const string Field3 = "";

        public static readonly int Field4;

        [CSharpDefinitionTest1(1)]
        public static readonly int Field5;


        public event EventHandler<EventArgs> Event1;

        public static event EventHandler Event2;

        [CSharpDefinitionTest1(1)]
        public static event EventHandler Event3;


        public int Property1 { get; set; }

        public byte Property2 { get; set; }

        public string Property3 { get; }

        public string Property4 { get; private set; }

        public string Property5 { private get; set; }

        public Stream Property6 { get; }

        public IEnumerable<string> Property7 { get; }

        public static IEnumerable<string> Property8 { get; }

        [CSharpDefinitionTest1(1)]
        public static IEnumerable<string> Property9 { get; }

        public int this[object parameter] { get { throw new NotImplementedException(); } }

        public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }


        public TestClass_CSharpDefinition()
        { }

        public TestClass_CSharpDefinition(string parameter)
        { }

        static TestClass_CSharpDefinition()
        {
        }

        public void Method1() => throw new NotImplementedException();

        public string Method2() => throw new NotImplementedException();

        public string Method3(string param1, Stream param2) => throw new NotImplementedException();

        public static string Method4(string param1, Stream param2) => throw new NotImplementedException();

        public static string Method5<TParam>(TParam parameter) => throw new NotImplementedException();

        [Obsolete]
        public void Method6() => throw new NotImplementedException();

        [Obsolete("Use another method")]
        public void Method7() => throw new NotImplementedException();

        [CSharpDefinitionTest1(1, Property1 = "Value")]
        public void Method8() => throw new NotImplementedException();

        [CSharpDefinitionTest2(CSharpDefinitionTestFlagsEnum.Value1 | CSharpDefinitionTestFlagsEnum.Value2)]
        public void Method9() => throw new NotImplementedException();

        [CSharpDefinitionTest3(BindingFlags.CreateInstance | BindingFlags.NonPublic)]
        public void Method10() => throw new NotImplementedException();

        [CSharpDefinitionTest4(CSharpDefinitionTestEnum.Value2)]
        public void Method11() => throw new NotImplementedException();

        public void Method12(ref int value) => throw new NotImplementedException();

        public void Method13(out string value) => throw new NotImplementedException();

        public void Method14(object parameter1, out string value) => throw new NotImplementedException();

        public void Method15(out string[] value) => throw new NotImplementedException();

        public void Method16(in string value) => throw new NotImplementedException();

        public void Method17(string stringParameter = "default") => throw new NotImplementedException();

        public void Method18(string stringParameter = null, int intParameter = 23) => throw new NotImplementedException();

        public void Method19(CSharpDefinitionTestEnum parameter = CSharpDefinitionTestEnum.Value1) => throw new NotImplementedException();

        public void Method20([Optional]string stringParameter) => throw new NotImplementedException();

        public void Method21([Optional][DefaultParameterValue("default")]string stringParameter) => throw new NotImplementedException();

        public void Method22([CSharpDefinitionTest5]string parameter1) => throw new NotImplementedException();

        public void Method23([CSharpDefinitionTest4(CSharpDefinitionTestEnum.Value2)][CSharpDefinitionTest5]string parameter1) => throw new NotImplementedException();

        public void Method24(params string[] parameters) => throw new NotImplementedException();


        public static TestClass_CSharpDefinition operator +(TestClass_CSharpDefinition other) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator +(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator -(TestClass_CSharpDefinition other) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator -(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator *(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator /(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator %(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator &(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator |(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator !(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator ~(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator ++(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator --(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static bool operator true(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static bool operator false(TestClass_CSharpDefinition left) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator <<(TestClass_CSharpDefinition left, int right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator >>(TestClass_CSharpDefinition left, int right) => throw new NotImplementedException();
        public static TestClass_CSharpDefinition operator ^(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator ==(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator !=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator <(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator >(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator <=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();
        public static bool operator >=(TestClass_CSharpDefinition left, TestClass_CSharpDefinition right) => throw new NotImplementedException();

        public static implicit operator string(TestClass_CSharpDefinition left) => throw new NotImplementedException();

        public static explicit operator int(TestClass_CSharpDefinition left) => throw new NotImplementedException();

    }

    public static class TestClass_CSharpDefinition_ExtensionMethods
    {
        public static void Method1(this string param) => throw new NotImplementedException();
    }

}
