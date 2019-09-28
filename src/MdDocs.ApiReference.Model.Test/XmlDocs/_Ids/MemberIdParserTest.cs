using System;
using System.Collections.Generic;
using System.Reflection;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Grynwald.MdDocs.ApiReference.Test.Model.XmlDocs
{
    public class MemberIdParserTest
    {
        public class MemberIdParserTestCase : IXunitSerializable
        {
            public string Input { get; private set; }

            public MemberId ExpectedMemberId { get; private set; }


            // parameterless constructor required by xunit
            public MemberIdParserTestCase()
            { }

            public MemberIdParserTestCase(string input, MemberId expectedMemberId)
            {
                Input = input;
                ExpectedMemberId = expectedMemberId;
            }


            public void Deserialize(IXunitSerializationInfo info)
            {
                Input = info.GetValue<string>(nameof(Input));
                ExpectedMemberId = info.GetValue<XunitSerializableMemberId>(nameof(ExpectedMemberId));
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(Input), Input);
                info.AddValue(nameof(ExpectedMemberId), new XunitSerializableMemberId(ExpectedMemberId));
            }

            public override string ToString() => Input;
        }


        public class MethodIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            private IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase(
                   "M:MdDoc.Test.TestData.TestClass.TestMethod1",
                   new MethodId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass"), "TestMethod1")
                );

                yield return new MemberIdParserTestCase(
                    "M:MdDoc.Test.TestData.TestClass.#ctor",
                    new MethodId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass"), ".ctor")
                );

                yield return new MemberIdParserTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1",
                    new MethodId(new GenericTypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1")
                );

                yield return new MemberIdParserTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1``2",
                    new MethodId(new GenericTypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1", 2, Array.Empty<TypeId>())
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.String,System.Int32)",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new SimpleTypeId("System", "String"), new SimpleTypeId("System", "Int32") })
                );

                yield return new MemberIdParserTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1(`0)",
                    new MethodId(
                        new GenericTypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1),
                        "TestMethod1",
                        0,
                        new[] { new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, 0) })
                );

                yield return new MemberIdParserTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1``2(``0,``1)",
                    new MethodId(
                        new GenericTypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1),
                        "TestMethod1",
                        2,
                        new[]
                        {
                            new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0),
                            new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 1)
                        })
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod``1(System.Collections.Generic.IEqualityComparer{``0})",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        1,
                        new[]
                        {
                            new GenericTypeInstanceId("System.Collections.Generic", "IEqualityComparer",
                            new []
                            {
                                new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, 0)
                            })
                        })
                );

                yield return new MemberIdParserTestCase(
                    "M:Class.Method(System.Collections.Generic.IEnumerable{System.String})",
                    new MethodId(
                        new SimpleTypeId("", "Class"),
                        "Method",
                        new[] { new GenericTypeInstanceId("System.Collections.Generic", "IEnumerable", new[] { new SimpleTypeId("System", "String") }) })
                );

                yield return new MemberIdParserTestCase(
                    "M:Class.Method(System.Collections.Generic.IEnumerable{System.Collections.Generic.IEnumerable{System.String}})",
                    new MethodId(
                        new SimpleTypeId("", "Class"),
                        "Method",
                        new[]
                        {
                            new GenericTypeInstanceId(
                                "System.Collections.Generic",
                                "IEnumerable",
                                new[]
                                {
                                    new GenericTypeInstanceId(
                                    "System.Collections.Generic",
                                    "IEnumerable",
                                    new [] { new SimpleTypeId("System", "String") }
                                    )
                                })
                        }
                ));

                yield return new MemberIdParserTestCase(
                    "M:Class.op_Implicit(Class)~System.String",
                    new MethodId(
                        new SimpleTypeId("", "Class"),
                        "op_Implicit",
                        0,
                        new[] { new SimpleTypeId("", "Class") },
                        new SimpleTypeId("System", "String")
                    )
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "Int32")) })
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[][])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new ArrayTypeId(new SimpleTypeId("System", "Int32"))) })
                );


                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.String[],System.Int32[])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "String")), new ArrayTypeId(new SimpleTypeId("System", "Int32")) })
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[,])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );


                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[0:,0:])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[0:1,0:1])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

                yield return new MemberIdParserTestCase(
                    "M:TestClass.TestMethod(System.Int32[,0:1])",
                    new MethodId(
                        new SimpleTypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

            }
        }

        public class PropertyIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            private IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Property1",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Property1")
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties`23.Property1",
                    new PropertyId(new GenericTypeId("MdDoc.Test.TestData", "TestClass_Properties", 23), "Property1")
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32)",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new SimpleTypeId("System", "Int32") })
                );

                yield return new MemberIdParserTestCase(
                    "P:Class`1.Item(`0)",
                    new PropertyId(
                        new GenericTypeId("", "Class", 1),
                        "Item",
                        new[] { new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, 0) })
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32[])",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new ArrayTypeId(new SimpleTypeId("System", "Int32")) })
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32[,])",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32[0:,0:])",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32[0:1,0:1])",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 2) })
                );

                yield return new MemberIdParserTestCase(
                    "P:MdDoc.Test.TestData.TestClass_Properties.Item(System.Int32[0:,0:,])",
                    new PropertyId(new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties"), "Item", new[] { new ArrayTypeId(new SimpleTypeId("System", "Int32"), 3) })
                );
            }
        }

        public class TypeIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            public IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase("T:System.String", new SimpleTypeId("System", "String"));
                yield return new MemberIdParserTestCase("T:System.String[]", new ArrayTypeId(new SimpleTypeId("System", "String")));
                yield return new MemberIdParserTestCase("T:System.String[,]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[0:,]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[0:,0:]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[0:5,0:]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[,0:6]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[,0:]", new ArrayTypeId(new SimpleTypeId("System", "String"), 2));
                yield return new MemberIdParserTestCase("T:System.String[,0:,,]", new ArrayTypeId(new SimpleTypeId("System", "String"), 4));
                yield return new MemberIdParserTestCase("T:System.String[,:5,,]", new ArrayTypeId(new SimpleTypeId("System", "String"), 4));
                yield return new MemberIdParserTestCase("T:System.String[][]", new ArrayTypeId(new ArrayTypeId(new SimpleTypeId("System", "String"))));
                yield return new MemberIdParserTestCase("T:System.Collections.Generic.IEnumerable`1", new GenericTypeId("System.Collections.Generic", "IEnumerable", 1));
                yield return new MemberIdParserTestCase("T:System.Collections.Generic.IEnumerable`1[]", new ArrayTypeId(new GenericTypeId("System.Collections.Generic", "IEnumerable", 1)));
                yield return new MemberIdParserTestCase("T:System.Collections.Generic.IEnumerable`23", new GenericTypeId("System.Collections.Generic", "IEnumerable", 23));
                yield return new MemberIdParserTestCase("T:System.Collections.Generic.IDictionary`2", new GenericTypeId("System.Collections.Generic", "IDictionary", 2));
            }
        }

        public class NamespaceIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            public IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase("N:System", new NamespaceId("System"));
                yield return new MemberIdParserTestCase("N:System.Collections", new NamespaceId("System.Collections"));
            }
        }

        public class FieldIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            public IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase("F:System.String.Length", new FieldId(new SimpleTypeId("System", "String"), "Length"));
                yield return new MemberIdParserTestCase("F:System.Foo.String.Length", new FieldId(new SimpleTypeId("System.Foo", "String"), "Length"));
                yield return new MemberIdParserTestCase("F:System.String`2.Length", new FieldId(new GenericTypeId("System", "String", 2), "Length"));
            }
        }

        public class EventIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            public IEnumerable<MemberIdParserTestCase> GetTestCases()
            {
                yield return new MemberIdParserTestCase("E:Namespace.Class.Event1", new EventId(new SimpleTypeId("Namespace", "Class"), "Event1"));
                yield return new MemberIdParserTestCase("E:Namespace.Namespace.Class.Event1", new EventId(new SimpleTypeId("Namespace.Namespace", "Class"), "Event1"));
                yield return new MemberIdParserTestCase("E:Namespace.Class`23.Event1", new EventId(new GenericTypeId("Namespace", "Class", 23), "Event1"));
            }
        }


        [Theory]
        [NamespaceIdTestCases]
        [TypeIdTestCases]
        [MethodIdTestCases]
        [PropertyIdTestCases]
        [EventIdTestCases]
        [FieldIdTestCases]
        public void Member_ids_are_parsed_as_expected(MemberIdParserTestCase testCase)
        {
            // ARRANGE
            var parser = new MemberIdParser(testCase.Input);

            // ACT
            var memberId = parser.Parse();

            // ASSERT
            Assert.NotNull(memberId);
            Assert.Equal(testCase.ExpectedMemberId, memberId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("X:")]
        [InlineData("T:System.Collections.Generic.IEnumerable`1foo")]
        [InlineData("T:System.Collections.Generic.IEnumerable`foo")]
        [InlineData("T:.Foo")]
        [InlineData("T:Foo[,)]")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1TestMethod1``2()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1``2()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1``2")]
        [InlineData("F:Name")]
        [InlineData("F:.Name")]
        [InlineData("N:")]
        public void Parse_throws_Exception_for_invalid_input_data(string input)
        {
            Assert.Throws<MemberIdParserException>(() => new MemberIdParser(input).Parse());
        }
    }
}
