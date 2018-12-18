using MdDoc.Model.XmlDocs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserTest
    {
        public class MethodIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                {
                    yield return new object[] { testCase };
                }

            }

            private IEnumerable<MemberIdParserMethodIdTestCase> GetTestCases()
            {
                yield return new MemberIdParserMethodIdTestCase(
                   "M:MdDoc.Test.TestData.TestClass.TestMethod1",
                   new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass"), "TestMethod1")
                );

                yield return new MemberIdParserMethodIdTestCase(
                    "M:MdDoc.Test.TestData.TestClass.#ctor",
                    new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass"), ".ctor")
                );

                yield return new MemberIdParserMethodIdTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1",
                    new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1")
                );

                yield return new MemberIdParserMethodIdTestCase(
                    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1``2",
                    new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1", 2, Array.Empty<TypeId>())
                );

                yield return new MemberIdParserMethodIdTestCase(
                    "M:TestClass.TestMethod(System.String,System.Int32)",
                    new MethodId(
                        new TypeId("", "TestClass"),
                        "TestMethod",
                        new TypeId[] { new TypeId("System", "String"), new TypeId("System", "Int32") })
                );

                //yield return new MemberIdParserMethodIdTestCase(
                //    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1(`0)",
                //    new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1", 0, new[] { "`0" })
                //);

                //yield return new MemberIdParserMethodIdTestCase(
                //    "M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1``2(``0)",
                //    new MethodId(new TypeId("MdDoc.Test.TestData", "TestClass_GenericType", 1), "TestMethod1", 2, new[] { "``0" })
                //);

                yield return new MemberIdParserMethodIdTestCase(
                    "M:Class.Method(System.Collections.Generic.IEnumerable{System.String})",
                    new MethodId(
                        new TypeId("", "Class"),
                        "Method",
                        new[] { new TypeId("System.Collections.Generic", "IEnumerable", new[] { new TypeId("System", "String") }) })
                );

                yield return new MemberIdParserMethodIdTestCase(
                    "M:Class.Method(System.Collections.Generic.IEnumerable{System.Collections.Generic.IEnumerable{System.String}})",
                    new MethodId(
                        new TypeId("", "Class"),
                        "Method",
                        new[]
                        {
                            new TypeId(
                                "System.Collections.Generic",
                                "IEnumerable",
                                new[]
                                {
                                    new TypeId(
                                    "System.Collections.Generic",
                                    "IEnumerable",
                                    new [] { new TypeId("System", "String") }
                                    )
                                })
                        }
                ));

                yield return new MemberIdParserMethodIdTestCase(
                    "M:Class.op_Implicit(Class)~System.String",
                    new MethodId(
                        new TypeId("", "Class"),
                        "op_Implicit",
                        0,
                        new[] { new TypeId("", "Class") },
                        new TypeId("System", "String")
                    )
                );

            }
        }

        public class TypeIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                {
                    yield return new object[] { testCase };
                }

            }

            public IEnumerable<MemberIdParserTypeIdTestCase> GetTestCases()
            {

                yield return new MemberIdParserTypeIdTestCase("T:System.String", new TypeId("System", "String"));
                yield return new MemberIdParserTypeIdTestCase("T:System.Collections.Generic.IEnumerable`1", new TypeId("System.Collections.Generic", "IEnumerable", 1));
                yield return new MemberIdParserTypeIdTestCase("T:System.Collections.Generic.IEnumerable`23", new TypeId("System.Collections.Generic", "IEnumerable", 23));
                yield return new MemberIdParserTypeIdTestCase("T:System.Collections.Generic.IDictionary`2", new TypeId("System.Collections.Generic", "IDictionary", 2));
            }
        }

        public class FieldIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                {
                    yield return new object[] { testCase };
                }

            }

            public IEnumerable<MemberIdParserFieldIdTestCase> GetTestCases()
            {
                yield return new MemberIdParserFieldIdTestCase("F:System.String.Length", new FieldId(new TypeId("System", "String"), "Length"));
                yield return new MemberIdParserFieldIdTestCase("F:System.Foo.String.Length", new FieldId(new TypeId("System.Foo", "String"), "Length"));
                yield return new MemberIdParserFieldIdTestCase("F:System.String`2.Length", new FieldId(new TypeId("System", "String", 2), "Length"));
            }
        }

        public class EventIdTestCasesAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                {
                    yield return new object[] { testCase };
                }

            }

            public IEnumerable<MemberIdParserEventIdTestCase> GetTestCases()
            {
                yield return new MemberIdParserEventIdTestCase("E:Namespace.Class.Event1", new EventId(new TypeId("Namespace", "Class"), "Event1"));
                yield return new MemberIdParserEventIdTestCase("E:Namespace.Namespace.Class.Event1", new EventId(new TypeId("Namespace.Namespace", "Class"), "Event1"));
                yield return new MemberIdParserEventIdTestCase("E:Namespace.Class`23.Event1", new EventId(new TypeId("Namespace", "Class", 23), "Event1"));
            }
        }


        [Theory]
        [TypeIdTestCases]
        public void Member_ids_for_types_are_parsed_as_expected(MemberIdParserTypeIdTestCase testCase)
        {
            var parser = new MemberIdParser(testCase.Input);
            var memberId = parser.Parse();

            Assert.NotNull(memberId);
            Assert.IsType<TypeId>(memberId);

            var typeId = (TypeId)memberId;

            AssertEqual(testCase.ExpectedTypeId, typeId);
        }

        [Theory]
        [MethodIdTestCases]
        public void Member_ids_for_methods_are_parsed_as_expected(MemberIdParserMethodIdTestCase testCase)
        {
            var parser = new MemberIdParser(testCase.Input);
            var memberId = parser.Parse();

            Assert.NotNull(memberId);
            Assert.IsType<MethodId>(memberId);

            var methodId = (MethodId)memberId;

            AssertEqual(testCase.ExpectedMethodId, methodId);
        }

        [Theory]
        [FieldIdTestCases]
        public void Member_ids_for_fields_are_parsed_as_expected(MemberIdParserFieldIdTestCase testCase)
        {
            var parser = new MemberIdParser(testCase.Input);
            var memberId = parser.Parse();

            Assert.NotNull(memberId);
            Assert.IsType<FieldId>(memberId);

            var fieldId = (FieldId)memberId;

            AssertEqual(testCase.ExpectedFieldId, fieldId);
        }

        [Theory]
        [EventIdTestCases]
        public void Member_ids_for_events_are_parsed_as_expected(MemberIdParserEventIdTestCase testCase)
        {
            var parser = new MemberIdParser(testCase.Input);
            var memberId = parser.Parse();

            Assert.NotNull(memberId);
            Assert.IsType<EventId>(memberId);

            var eventId = (EventId)memberId;

            AssertEqual(testCase.ExpectedEventId, eventId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("X:")]
        [InlineData("T:System.Collections.Generic.IEnumerable`1foo")]
        [InlineData("T:System.Collections.Generic.IEnumerable`foo")]
        [InlineData("T:.Foo")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1TestMethod1``2()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1.TestMethod1()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1``2()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1()")]
        [InlineData("M:MdDoc.Test.TestData.TestClass_GenericType`1``2")]
        [InlineData("F:Name")]
        [InlineData("F:.Name")]
        public void Parse_throws_Exception_for_invalid_input_data(string input)
        {
            Assert.Throws<MemberIdParserException>(() => new MemberIdParser(input).Parse());
        }


        private static void AssertEqual(TypeId expected, TypeId actual)
        {
            Assert.Equal(expected.NamespaceName, actual.NamespaceName);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Arity, actual.Arity);

            Assert.Equal(expected.TypeArguments.Count, actual.TypeArguments.Count);
            for (int i = 0; i < expected.TypeArguments.Count; i++)
            {
                AssertEqual(expected.TypeArguments[i], actual.TypeArguments[i]);
            }

        }

        private static void AssertEqual(MethodId expected, MethodId actual)
        {
            AssertEqual(expected.DefiningType, actual.DefiningType);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Arity, actual.Arity);

            Assert.Equal(expected.Parameters.Count, actual.Parameters.Count);
            for (int i = 0; i < expected.Parameters.Count; i++)
            {
                AssertEqual(expected.Parameters[i], actual.Parameters[i]);
            }

            if (expected.ReturnType == null)
            {
                Assert.Null(actual.ReturnType);
            }
            else
            {
                AssertEqual(expected.ReturnType, actual.ReturnType);
            }
        }

        private static void AssertEqual(FieldId expected, FieldId actual)
        {
            AssertEqual(expected.DefiningType, actual.DefiningType);

            Assert.Equal(expected.Name, actual.Name);
        }

        private static void AssertEqual(EventId expected, EventId actual)
        {
            AssertEqual(expected.DefiningType, actual.DefiningType);

            Assert.Equal(expected.Name, actual.Name);
        }
    }
}
