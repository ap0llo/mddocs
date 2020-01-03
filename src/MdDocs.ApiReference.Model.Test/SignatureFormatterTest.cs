using System.Linq;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class SignatureFormatterTest : DynamicCompilationTestBase
    {
        [Theory]
        [InlineData("public void Method1()", "Method1()")]
        [InlineData("public string Method2()", "Method2()")]
        [InlineData("public string Method3(string foo)", "Method3(string)")]
        [InlineData("public string Method4(IDisposable foo)", "Method4(IDisposable)")]
        [InlineData("public T Method5<T>(string foo)", "Method5<T>(string)")]
        [InlineData("public object Method6<T>(T foo)", "Method6<T>(T)")]
        [InlineData("public object Method7<T1, T2>(T1 foo, T2 bar)", "Method7<T1, T2>(T1, T2)")]
        [InlineData("public int? Method8(ConsoleColor? parameter)", "Method8(ConsoleColor?)")]
        [InlineData("public void Method9(ref string value)", "Method9(string)")]
        [InlineData("public void Method10(out string value)", "Method10(string)")]
        [InlineData("public void Method11(in string value)", "Method11(string)")]
        public void GetSignature_returns_the_expected_result(string methodDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    {methodDefinition} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => !x.IsConstructor);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public void Method1()", "Method1()")]
        [InlineData("public string Method2()", "Method2()")]
        [InlineData("public string Method3(string foo)", "Method3(string)")]
        [InlineData("public string Method4(IDisposable foo)", "Method4(IDisposable)")]
        [InlineData("public T Method5<T>(string foo)", "Method5<T>(string)")]
        [InlineData("public object Method6<T>(T foo)", "Method6<T>(T)")]
        [InlineData("public object Method7<T1, T2>(T1 foo, T2 bar)", "Method7<T1, T2>(T1, T2)")]
        [InlineData("public int? Method8(ConsoleColor? parameter)", "Method8(ConsoleColor?)")]
        [InlineData("public void Method9(ref string value)", "Method9(string)")]
        [InlineData("public void Method10(out string value)", "Method10(string)")]
        [InlineData("public void Method11(in string value)", "Method11(string)")]
        public void GetSignature_returns_the_expected_result_for_method_ids(string methodDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    {methodDefinition} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => !x.IsConstructor);


            var methodId = method.ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public Class1()", "Class1()")]
        [InlineData("public Class1(string foo)", "Class1(string)")]
        [InlineData("public Class1(string foo, IEnumerable<string> bar)", "Class1(string, IEnumerable<string>)")]
        [InlineData("public Class1(string foo, IEnumerable<string> bar, IList<DirectoryInfo> baz)", "Class1(string, IEnumerable<string>, IList<DirectoryInfo>)")]
        public void GetSignature_returns_the_expected_result_for_constructors(string ctorDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class Class1
                {{
                    {ctorDefinition}
                    {{ }}
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.IsConstructor);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public Class1()", "Class1()")]
        [InlineData("public Class1(string parameter)", "Class1(string)")]
        public void GetSignature_returns_the_expected_result_for_constructors_of_generic_types(string ctorDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;
                using System.Collections.Generic;

                public class Class1<T>
                {{
                    {ctorDefinition}
                    {{ }}
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`1")
                .Methods
                .Single(x => x.IsConstructor);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public Class1()", "Class1()")]
        [InlineData("public Class1(string foo)", "Class1(string)")]
        [InlineData("public Class1(string foo, IEnumerable<string> bar)", "Class1(string, IEnumerable<string>)")]
        [InlineData("public Class1(string foo, IEnumerable<string> bar, IList<DirectoryInfo> baz)", "Class1(string, IEnumerable<string>, IList<DirectoryInfo>)")]
        public void GetSignature_returns_the_expected_result_for_constructors_as_method_ids(string ctorDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class Class1
                {{
                    {ctorDefinition}
                    {{ }}
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.IsConstructor);

            var methodId = method.ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public Class1()", "Class1()")]
        [InlineData("public Class1(string parameter)", "Class1(string)")]
        public void GetSignature_returns_the_expected_result_for_constructors_of_generic_types_as_method_ids(string ctorDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;
                using System.Collections.Generic;

                public class Class1<T>
                {{
                    {ctorDefinition}
                    {{ }}
                }}
            ";

            using var assembly = Compile(cs);

            var ctor = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`1")
                .Methods
                .Single(x => x.IsConstructor);

            var methodId = ctor.ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("op_Addition", "Addition(Class1, Class1)")]
        [InlineData("op_Implicit", "Implicit(Class1 to string)")]
        public void GetSignature_returns_the_expected_result_for_operators(string methodName, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static implicit operator string(Class1 instance) => throw new NotImplementedException();        
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == methodName);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("op_Addition", "Addition(Class1, Class1)")]
        [InlineData("op_Implicit", "Implicit(Class1 to string)")]
        public void GetSignature_returns_the_expected_result_for_operators_as_method_ids(string methodName, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static implicit operator string(Class1 instance) => throw new NotImplementedException();        
                }}
            ";

            using var assembly = Compile(cs);

            var method = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == methodName);


            var methodId = method.ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public int this[int parameter]", "Item[int]")]
        [InlineData("public int this[int x, int y]", "Item[int, int]")]
        public void GetSignature_returns_the_expected_result_for_indexers(string indexerDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    {indexerDefinition} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var property = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(property);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("public int this[int parameter]", "Item[int]")]
        [InlineData("public int this[int x, int y]", "Item[int, int]")]
        public void GetSignature_returns_the_expected_result_for_indexers_as_property_id(string indexerDefinition, string expectedSignature)
        {
            // ARRANGE
            var cs = $@"
                using System;

                public class Class1
                {{
                    {indexerDefinition} => throw new NotImplementedException();
                }}
            ";

            using var assembly = Compile(cs);

            var property = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single();

            var propertyId = property.ToPropertyId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(propertyId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }
    }
}
