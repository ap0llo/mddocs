using System;
using System.Collections;
using System.Reflection;

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


    }

}
