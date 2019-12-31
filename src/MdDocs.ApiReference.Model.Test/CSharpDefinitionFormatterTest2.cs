using System.Linq;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class CSharpDefinitionFormatterTest2 : DynamicCompilationTestBase
    {
        [Fact]
        public void GetDefinition_does_not_include_internal_attributes()
        {
            // ARRANGE
            var cs = @"
                using System;

                internal class MyInternalAttribute : Attribute
                { }

                [MyInternalAttribute]
                public class MyClass
                {
                    [MyInternalAttribute]
                    public string Property1 { get; }

                    [MyInternalAttribute]
                    public string this[int index] => throw new NotImplementedException();

                    [MyInternalAttribute]
                    public int Field1;

                    [MyInternalAttribute]
                    public static event EventHandler Event1;

                    [MyInternalAttribute]
                    public void Method1([MyInternalAttribute]string parameter) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.GetTypes().Single(x => x.Name == "MyClass");
            var propertyDefinition = typeDefinition.Properties.Single(x => !x.HasParameters);
            var indexerDefinition = typeDefinition.Properties.Single(x => x.HasParameters);
            var fieldDefinition = typeDefinition.Fields.Single(x => x.IsPublic);
            var eventDefinition = typeDefinition.Events.Single();
            var methodDefinition = typeDefinition.Methods.Single(x => x.Name == "Method1");

            // ACT / ASSERT
            Assert.Equal("public class MyClass", CSharpDefinitionFormatter.GetDefinition(typeDefinition));
            Assert.Equal("public string Property1 { get; }", CSharpDefinitionFormatter.GetDefinition(propertyDefinition));
            Assert.Equal("public string this[int index] { get; }", CSharpDefinitionFormatter.GetDefinition(indexerDefinition));
            Assert.Equal("public int Field1;", CSharpDefinitionFormatter.GetDefinition(fieldDefinition));
            Assert.Equal("public static event EventHandler Event1;", CSharpDefinitionFormatter.GetDefinition(eventDefinition));
            Assert.Equal("public void Method1(string parameter);", CSharpDefinitionFormatter.GetDefinition(methodDefinition));
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_nested_types()
        {
            // ARRANGE
            var cs = @"

                public class Class1
                {
                    public class NestedClass1
                    {
                        public class NestedClass2
                        { }
                    }

                    public interface NestedInterface1
                    { }

                    public class NestedClass3<T>
                    {
                        public class NestedClass4
                        { }
                    }

                    public class NestedClass5<T1, T2>
                    {
                        public class NestedClass6<T3>
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var nestedClass1 = class1.NestedTypes.Single(x => x.Name == "NestedClass1");
            var nestedClass2 = nestedClass1.NestedTypes.Single(x => x.Name == "NestedClass2");
            var nestedInterface1 = class1.NestedTypes.Single(x => x.Name == "NestedInterface1");
            var nestedClass3 = class1.NestedTypes.Single(x => x.Name == "NestedClass3`1");
            var nestedClass4 = nestedClass3.NestedTypes.Single(x => x.Name == "NestedClass4");
            var nestedClass5 = class1.NestedTypes.Single(x => x.Name == "NestedClass5`2");
            var nestedClass6 = nestedClass5.NestedTypes.Single(x => x.Name == "NestedClass6`1");


            // ACT / ASSERT
            Assert.Equal("public class Class1.NestedClass1", CSharpDefinitionFormatter.GetDefinition(nestedClass1));
            Assert.Equal("public class Class1.NestedClass1.NestedClass2", CSharpDefinitionFormatter.GetDefinition(nestedClass2));
            Assert.Equal("public interface Class1.NestedInterface1", CSharpDefinitionFormatter.GetDefinition(nestedInterface1));
            Assert.Equal("public class Class1.NestedClass3<T>", CSharpDefinitionFormatter.GetDefinition(nestedClass3));
            Assert.Equal("public class Class1.NestedClass3<T>.NestedClass4", CSharpDefinitionFormatter.GetDefinition(nestedClass4));
            Assert.Equal("public class Class1.NestedClass5<T1, T2>", CSharpDefinitionFormatter.GetDefinition(nestedClass5));
            Assert.Equal("public class Class1.NestedClass5<T1, T2>.NestedClass6<T3>", CSharpDefinitionFormatter.GetDefinition(nestedClass6));

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_properties()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public int Property1 { get; set; }

                    public byte Property2 { get; set; }

                    public string Property3 { get; }

                    public string Property4 { get; private set; }

                    public string Property5 { private get; set; }

                    public Stream Property6 { get; }

                    public IEnumerable<string> Property7 { get; }

                    public static IEnumerable<string> Property8 { get; }

                    [Sample(1)]
                    public static IEnumerable<string> Property9 { get; }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var property1 = class1.Properties.Single(x => x.Name == "Property1");
            var property2 = class1.Properties.Single(x => x.Name == "Property2");
            var property3 = class1.Properties.Single(x => x.Name == "Property3");
            var property4 = class1.Properties.Single(x => x.Name == "Property4");
            var property5 = class1.Properties.Single(x => x.Name == "Property5");
            var property6 = class1.Properties.Single(x => x.Name == "Property6");
            var property7 = class1.Properties.Single(x => x.Name == "Property7");
            var property8 = class1.Properties.Single(x => x.Name == "Property8");
            var property9 = class1.Properties.Single(x => x.Name == "Property9");

            // ACT / ASSERT
            Assert.Equal("public int Property1 { get; set; }", CSharpDefinitionFormatter.GetDefinition(property1));
            Assert.Equal("public byte Property2 { get; set; }", CSharpDefinitionFormatter.GetDefinition(property2));
            Assert.Equal("public string Property3 { get; }", CSharpDefinitionFormatter.GetDefinition(property3));
            Assert.Equal("public string Property4 { get; }", CSharpDefinitionFormatter.GetDefinition(property4));
            Assert.Equal("public string Property5 { set; }", CSharpDefinitionFormatter.GetDefinition(property5));
            Assert.Equal("public Stream Property6 { get; }", CSharpDefinitionFormatter.GetDefinition(property6));
            Assert.Equal("public IEnumerable<string> Property7 { get; }", CSharpDefinitionFormatter.GetDefinition(property7));
            Assert.Equal("public static IEnumerable<string> Property8 { get; }", CSharpDefinitionFormatter.GetDefinition(property8));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static IEnumerable<string> Property9 { get; }",
                CSharpDefinitionFormatter.GetDefinition(property9)
            );
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_fields()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public string Field1;

                    public static string Field2;

                    public const string Field3 = """";

                    public static readonly int Field4;

                    [Sample(1)]
                    public static readonly int Field5;
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var field1 = class1.Fields.Single(x => x.Name == "Field1");
            var field2 = class1.Fields.Single(x => x.Name == "Field2");
            var field3 = class1.Fields.Single(x => x.Name == "Field3");
            var field4 = class1.Fields.Single(x => x.Name == "Field4");
            var field5 = class1.Fields.Single(x => x.Name == "Field5");

            // ACT / ASSERT
            Assert.Equal("public string Field1;", CSharpDefinitionFormatter.GetDefinition(field1));
            Assert.Equal("public static string Field2;", CSharpDefinitionFormatter.GetDefinition(field2));
            Assert.Equal("public const string Field3;", CSharpDefinitionFormatter.GetDefinition(field3));
            Assert.Equal("public static readonly int Field4;", CSharpDefinitionFormatter.GetDefinition(field4));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static readonly int Field5;",
                CSharpDefinitionFormatter.GetDefinition(field5));
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_events()
        {
            // ARRANGE
            var cs = @"
                using System;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public event EventHandler<EventArgs> Event1;

                    public static event EventHandler Event2;

                    [Sample(1)]
                    public static event EventHandler Event3;

                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var event1 = class1.Events.Single(x => x.Name == "Event1");
            var event2 = class1.Events.Single(x => x.Name == "Event2");
            var event3 = class1.Events.Single(x => x.Name == "Event3");

            // ACT / ASSERT
            Assert.Equal("public event EventHandler<EventArgs> Event1;", CSharpDefinitionFormatter.GetDefinition(event1));
            Assert.Equal("public static event EventHandler Event2;", CSharpDefinitionFormatter.GetDefinition(event2));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static event EventHandler Event3;",
                CSharpDefinitionFormatter.GetDefinition(event3)
            );

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_indexers()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;

                public class Class1
                {
                    public int this[object parameter] { get { throw new NotImplementedException(); } }

                    public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var indexer1 = class1.Properties.Single(p => p.Parameters.Count == 1);
            var indexer2 = class1.Properties.Single(p => p.Parameters.Count == 2);

            // ACT / ASSERT
            Assert.Equal("public int this[object parameter] { get; }", CSharpDefinitionFormatter.GetDefinition(indexer1));
            Assert.Equal("public int this[object parameter1, Stream parameter2] { get; }", CSharpDefinitionFormatter.GetDefinition(indexer2));

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_extension_methods()
        {
            // ARRANGE
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;

                public static class Class1
                {
                    public static void Method1(this string param) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method1 = class1.Methods.Single(x => x.Name == "Method1");

            // ACT / ASSERT
            Assert.Equal("public static void Method1(this string param);", CSharpDefinitionFormatter.GetDefinition(method1));
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_constructors()
        {
            // ARRANGE
            var cs = @"
                public class Class1
                {
                    public Class1()
                    { }

                    public Class1(string parameter)
                    { }

                    static Class1()
                    {
                    }
                }

                public class Class2<TParam>
                {
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var ctor11 = class1.Methods.Single(x => x.IsConstructor && !x.IsStatic && x.Parameters.Count == 0);
            var ctor12 = class1.Methods.Single(x => x.IsConstructor && !x.IsStatic && x.Parameters.Count == 1);
            var staticCtor = class1.Methods.Single(x => x.IsConstructor && x.IsStatic);
            var class2 = assembly.MainModule.Types.Single(x => x.Name == "Class2`1");
            var ctor21 = class2.Methods.Single(x => x.IsConstructor);

            // ACT / ASSERT
            Assert.Equal("public Class1();", CSharpDefinitionFormatter.GetDefinition(ctor11));
            Assert.Equal("public Class1(string parameter);", CSharpDefinitionFormatter.GetDefinition(ctor12));
            Assert.Equal("static Class1();", CSharpDefinitionFormatter.GetDefinition(staticCtor));
            Assert.Equal("public Class2();", CSharpDefinitionFormatter.GetDefinition(ctor21));
        }

        [Theory]
        [InlineData("op_UnaryPlus", "public static Class1 operator +(Class1 other);")]
        [InlineData("op_Addition", "public static Class1 operator +(Class1 left, Class1 right);")]
        [InlineData("op_UnaryNegation", "public static Class1 operator -(Class1 other);")]
        [InlineData("op_Subtraction", "public static Class1 operator -(Class1 left, Class1 right);")]
        [InlineData("op_Multiply", "public static Class1 operator *(Class1 left, Class1 right);")]
        [InlineData("op_Division", "public static Class1 operator /(Class1 left, Class1 right);")]
        [InlineData("op_Modulus", "public static Class1 operator %(Class1 left, Class1 right);")]
        [InlineData("op_BitwiseAnd", "public static Class1 operator &(Class1 left, Class1 right);")]
        [InlineData("op_BitwiseOr", "public static Class1 operator |(Class1 left, Class1 right);")]
        [InlineData("op_LogicalNot", "public static Class1 operator !(Class1 left);")]
        [InlineData("op_OnesComplement", "public static Class1 operator ~(Class1 left);")]
        [InlineData("op_Increment", "public static Class1 operator ++(Class1 left);")]
        [InlineData("op_Decrement", "public static Class1 operator --(Class1 left);")]
        [InlineData("op_True", "public static bool operator true(Class1 left);")]
        [InlineData("op_False", "public static bool operator false(Class1 left);")]
        [InlineData("op_LeftShift", "public static Class1 operator <<(Class1 left, int right);")]
        [InlineData("op_RightShift", "public static Class1 operator >>(Class1 left, int right);")]
        [InlineData("op_ExclusiveOr", "public static Class1 operator ^(Class1 left, Class1 right);")]
        [InlineData("op_Equality", "public static bool operator ==(Class1 left, Class1 right);")]
        [InlineData("op_Inequality", "public static bool operator !=(Class1 left, Class1 right);")]
        [InlineData("op_LessThan", "public static bool operator <(Class1 left, Class1 right);")]
        [InlineData("op_GreaterThan", "public static bool operator >(Class1 left, Class1 right);")]
        [InlineData("op_LessThanOrEqual", "public static bool operator <=(Class1 left, Class1 right);")]
        [InlineData("op_GreaterThanOrEqual", "public static bool operator >=(Class1 left, Class1 right);")]
        [InlineData("op_Implicit", "public static implicit operator string(Class1 left);")]
        [InlineData("op_Explicit", "public static explicit operator int(Class1 left);")]
        public void GetDefinition_returns_the_expected_definition_for_operators(string methodName, string expected)
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

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(p => p.Name == methodName);
                
            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("public void Method1();")]
        [InlineData("public string Method2();")]
        [InlineData("public string Method3(string param1, Stream param2);")]
        [InlineData("public static string Method4(string param1, Stream param2);")]
        [InlineData("public static string Method5<TParam>(TParam parameter);")]
        public void GetDefinition_returns_the_expected_definition_for_methods(string expected)
        {
            // ARRANGE
            var cs = @$"
                using System;
                using System.IO;
                using System.Runtime.InteropServices;
               
                public class Class1
                {{
                    {expected.Trim(';')} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(m => !m.IsConstructor);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("[Obsolete]\r\npublic void Method6();")]
        [InlineData("[Obsolete(\"Use another method\")]\r\npublic void Method7();")]
        [InlineData("[Test1(1, Property1 = \"Value\")]\r\npublic void Method8();")]
        [InlineData("[Test2(TestEnum1.Value1 | TestEnum1.Value2)]\r\npublic void Method9();")]
        [InlineData("[Test3(BindingFlags.NonPublic | BindingFlags.CreateInstance)]\r\npublic void Method10();")]
        [InlineData("[Test4(TestEnum2.Value2)]\r\npublic void Method11();")]
        public void GetDefinition_returns_the_expected_definition_for_methods_with_attributes(string expected)
        {
            // ARRANGE
            var cs = @$"
                using System;
                using System.Reflection;

                public class Test1Attribute : Attribute
                {{
	                public string Property1 {{ get; set; }}

	                public Test1Attribute(int value)
	                {{ }}
                }}

                public class Test2Attribute : Attribute
                {{
	                public Test2Attribute(TestEnum1 value)
	                {{ }}
                }}

                public class Test3Attribute : Attribute
                {{
	                public Test3Attribute(BindingFlags value)
	                {{ }}
                }}


                public class Test4Attribute : Attribute
                {{
	                public Test4Attribute(TestEnum2 value)
	                {{ }}
                }}

                [Flags]
                public enum TestEnum1 : short
                {{
	                Value1 = 0x01,
	                Value2 = 0x01 << 1,
	                Value3 = 0x01 << 2
                }}

                public enum TestEnum2
                {{
	                Value1 = 1,
	                Value2 = 2,
	                Value3 = 3,
                }}


                public class Class1
                {{
                    {expected.Trim(';')} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(m => !m.IsConstructor);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("public void Method22([Test1]string parameter1);")]
        [InlineData("public void Method23([Test2(TestEnum1.Value2)][Test1]string parameter1);")]
        public void GetDefinition_returns_the_expected_definition_for_methods_with_parameter_attributes(string expected)
        {
            // ARRANGE
            var cs = @$"
                using System;               

                public class Test1Attribute : Attribute
                {{
                }}

                public class Test2Attribute : Attribute
                {{
	                public Test2Attribute(TestEnum1 value)
	                {{ }}
                }}

                [Flags]
                public enum TestEnum1 : short
                {{
	                Value1 = 0x01,
	                Value2 = 0x01 << 1,
	                Value3 = 0x01 << 2
                }}

                public class Class1
                {{
                    {expected.Trim(';')} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(m => !m.IsConstructor);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("public void MethodName(ref int value);")]
        [InlineData("public void MethodName(out string value);")]
        [InlineData("public void MethodName(object parameter1, out string value);")]
        [InlineData("public void MethodName(out string[] value);")]
        [InlineData("public void MethodName(in string value);")]        
        public void GetDefinition_returns_the_expected_definition_for_methods_with_ref_parameters(string expected)
        {
            // ARRANGE
            var cs = $@"
                using System;
              
                public class Class1
                {{
                    {expected.Trim(';')} => throw new NotImplementedException();
                }}

            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(m => !m.IsConstructor);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("public void Method(string stringParameter = \"default\");")]
        [InlineData("public void Method(string stringParameter = null, int intParameter = 23);")]
        [InlineData("public void Method(Enum1 parameter = Enum1.Value1);")]
        [InlineData("public void Method([Optional]string stringParameter);")]      
        public void GetDefinition_returns_the_expected_definition_for_methods_with_optional_parameters(string expected)
        {
            // ARRANGE
            var cs = @$"
                using System;
                using System.Runtime.InteropServices;
               
                public enum Enum1
                {{
                    Value1 = 1,
                    Value2 = 2,
                    Value3 = 3,
                }}

                public class Class1
                {{
                    {expected.Trim(';')} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method = class1.Methods.Single(m => !m.IsConstructor);

            // ACT
            var actual = CSharpDefinitionFormatter.GetDefinition(method);

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_methods_using_params()
        {
            // ARRANGE
            var cs = @"
                using System;
              
                public class Class1
                {                   
                    public void Method1(params string[] parameters) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method1 = class1.Methods.Single(p => p.Name == "Method1");

            // ACT / ASSERT
            Assert.Equal("public void Method1(params string[] parameters);", CSharpDefinitionFormatter.GetDefinition(method1));
        }
    }
}
