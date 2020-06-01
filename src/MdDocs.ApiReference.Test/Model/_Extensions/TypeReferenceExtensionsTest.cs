using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class TypeReferenceExtensionsTest : DynamicCompilationTestBase
    {
        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_01()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                    }
                }
            ";

            using var assembly = Compile(cs);


            var typeReference = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var expectedMemberId = new SimpleTypeId("Namespace1.Namespace2", "Class1");

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_type_definitions_02()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1<T1>
                    {
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types.Single(x => x.Name == "Class1`1");
            var expectedMemberId = new GenericTypeId("Namespace1.Namespace2", "Class1", 1);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_constructued_types_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public string Method1(IEnumerable<string> bar) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new GenericTypeInstanceId(
                "System.Collections.Generic",
                "IEnumerable",
                new[] { new SimpleTypeId("System", "String") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_constructued_types_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public string Method1<T>(IEnumerable<ArraySegment<T>> bar) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new GenericTypeInstanceId(
                "System.Collections.Generic",
                "IEnumerable",
                new[]
                {
                    new GenericTypeInstanceId(
                        new NamespaceId("System"),
                        "ArraySegment",
                        new []{ new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0) })
                }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(string[] parameter) { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(string[][] parameter) { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new ArrayTypeId(new SimpleTypeId("System", "String")));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_array_types_03()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(string[,] parameter) { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ArrayTypeId(new SimpleTypeId("System", "String"), 2);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public T2 Method1<T1, T2>(T2 foo, T1 bar) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_generic_parameters_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1<T1>
                    {
                        public void Method1(T1 foo) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .First()
                .ParameterType;

            var expectedMemberId = new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, 0);

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_ref_parameters_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public bool Method1(ref string value) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);

        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_ref_parameters_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public bool Method1(ref string[] value) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            var expectedMemberId = new ByReferenceTypeId(new ArrayTypeId(new SimpleTypeId("System", "String")));

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);

        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_out_parameters()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public bool Method1(out string value) => throw new NotImplementedException(); 
                    }
                }
            ";

            using var assembly = Compile(cs);

            var parameter = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single();

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = parameter.ParameterType.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
            Assert.Equal(ParameterAttributes.Out, parameter.Attributes);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_in_parameters()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public bool Method1(in string value) => throw new NotImplementedException();
                    }
                }
            ";

            using var assembly = Compile(cs);

            var parameter = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single();

            var expectedMemberId = new ByReferenceTypeId(new SimpleTypeId("System", "String"));

            // ACT
            var actualMemberId = parameter.ParameterType.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
            Assert.Equal(ParameterAttributes.In, parameter.Attributes);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public class NestedClass1
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .NestedTypes
                .Single(x => x.Name == "NestedClass1");

            var expectedId = new SimpleTypeId(
                new SimpleTypeId("Namespace1.Namespace2", "Class1"),
                "NestedClass1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public class NestedClass1
                        {
                            public class NestedClass2
                            { }
                        }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .NestedTypes
                .Single(x => x.Name == "NestedClass1")
                .NestedTypes
                .Single(x => x.Name == "NestedClass2");

            var expectedId = new SimpleTypeId(
                new SimpleTypeId(
                    new SimpleTypeId("Namespace1.Namespace2", "Class1"),
                    "NestedClass1"),
                "NestedClass2"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_03()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public interface NestedInterface1
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .NestedTypes
                .Single(x => x.Name == "NestedInterface1");

            var expectedId = new SimpleTypeId(
                new SimpleTypeId("Namespace1.Namespace2", "Class1"),
                "NestedInterface1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_04()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public class NestedClass1<T>
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .NestedTypes
                .Single();

            var expectedId = new GenericTypeId(
                new SimpleTypeId("Namespace1.Namespace2", "Class1"),
                "NestedClass1",
                1
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_05()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1<T1>
                    {
                        public class NestedClass1
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`1")
                .NestedTypes
                .Single(x => x.Name == "NestedClass1");

            var expectedId = new SimpleTypeId(
                new GenericTypeId("Namespace1.Namespace2", "Class1", 1),
                "NestedClass1"
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_for_nested_types_06()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1<T1>
                    {
                        public class NestedClass1
                        {
                            public class NestedClass2<T2>
                            { }
                        }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`1")
                .NestedTypes
                .Single()
                .NestedTypes
                .Single();

            var expectedId = new GenericTypeId(
                new SimpleTypeId(
                    new GenericTypeId("Namespace1.Namespace2", "Class1", 1),
                    "NestedClass1"),
                "NestedClass2",
                1
            );

            // ACT
            var actualId = typeDefinition.ToMemberId();

            // ASSERT
            Assert.NotNull(actualId);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_01()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(Class2.NestedClass1<string> parameter) => throw new NotImplementedException();
                    }

                    public class Class2
                    {
                        public class NestedClass1<T2>
                        { }
                    }
    
                }

            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;


            // type: Class2.NestedClass1<string>  parameter
            var expectedMemberId = new GenericTypeInstanceId(
                new SimpleTypeId("Namespace1.Namespace2", "Class2"),
                "NestedClass1",
                new[] { new SimpleTypeId("System", "String") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_02()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(Class2<string>.NestedClass1 parameter) => throw new NotImplementedException();
                    }

                    public class Class2<T>
                    {
                        public class NestedClass1
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            // type: TestClass_NestedTypes<string>.NestedClass1
            var expectedMemberId = new SimpleTypeId(
                new GenericTypeInstanceId("Namespace1.Namespace2", "Class2",
                    new[] { new SimpleTypeId("System", "String") }),
                "NestedClass1"
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_expected_value_for_nested_constructued_types_03()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1(Class2<string>.NestedClass1.NestedClass2<int> parameter) => throw new NotImplementedException();
                    }

                    public class Class2<T1>
                    {
                        public class NestedClass1
                        {
                            public class NestedClass2<T2>
                            { }
                        }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Methods
                .Single(x => x.Name == "Method1")
                .Parameters
                .Single()
                .ParameterType;

            // type: TestClass_NestedTypes<string>.NestedClass1.NestedClass2<int>
            var expectedMemberId = new GenericTypeInstanceId(
                new SimpleTypeId(
                    new GenericTypeInstanceId(
                        "Namespace1.Namespace2",
                        "Class2",
                        new[] { new SimpleTypeId("System", "String") }),
                    "NestedClass1"),
                "NestedClass2",
                new[] { new SimpleTypeId("System", "Int32") }
            );

            // ACT
            var actualMemberId = typeReference.ToMemberId();

            // ASSERT
            Assert.NotNull(actualMemberId);
            Assert.Equal(expectedMemberId, actualMemberId);
        }
    }
}
