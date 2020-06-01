using System;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model.XmlDocs
{
    public class XmlDocsProviderTest : DynamicCompilationTestBase
    {

        [Fact]
        public void TryGetDocumentationComments_returns_null_for_type_without_documentation()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.Null(docs);
        }

        [Fact]
        public void TryGetDocumentationComments_returns_null_for_method_without_documentation()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public void Method1() => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new MethodId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Method1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.Null(docs);
        }

        [Fact]
        public void TryGetDocumentationComments_returns_null_for_field_without_documentation()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public string Field1;
                }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new FieldId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Field1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.Null(docs);
        }

        [Fact]
        public void TryGetDocumentationComments_returns_null_for_property_without_documentation()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public string Property1 { get; }
                }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new PropertyId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Property1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.Null(docs);
        }

        [Fact]
        public void TryGetDocumentationComments_returns_null_for_event_without_documentation()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public event EventHandler Event1;
                }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new EventId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Event1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var summary = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.Null(summary);
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

                /// <summary>
                /// This is a test class for testing parsing of XML documentation
                /// </summary>
                /// <remarks>Remarks</remarks>
                /// <example>Example1</example>    
                /// <typeparam name=""T1"">Type parameter 1</typeparam>
                /// <typeparam name=""T2"">Type parameter 2</typeparam>
                /// <seealso  cref=""Class2"">Some text</seealso>
                /// <seealso  cref=""Interface1"" />
                public class Class1<T1, T2>
	            { }

                public class Class2
                { }

                public interface Interface1
                { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new GenericTypeId(NamespaceId.GlobalNamespace, "Class1", 2);

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<GenericTypeId>(docs.MemberId);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.TypeParameters);
            Assert.Equal(2, docs.TypeParameters.Count);
            Assert.Contains(docs.TypeParameters, x => x.Key == "T1");
            Assert.Contains(docs.TypeParameters, x => x.Key == "T2");

            Assert.All(docs.SeeAlso, seeAlso => Assert.NotNull(seeAlso.MemberId));

            Assert.Equal(2, docs.SeeAlso.Count);
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 1 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"))
            );
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 0 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"))
            );
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_method()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    /// <summary>
                    /// Line1
                    /// </summary>
                    /// <remarks>Remarks</remarks>
                    /// <example>Example</example>
                    /// <exception cref=""InvalidOperationException"">Exception 1</exception>
                    /// <exception cref=""ArgumentException"">Exception 2</exception>
                    /// <param name=""parameter1"">Documentation for parameter 1</param>
                    /// <returns>The method returns an object</returns>
                    /// <seealso cref=""Class2"">Some text</seealso>
                    /// <seealso cref=""Interface1"" />
                    public void Method1() => throw new NotImplementedException();
                }

                public class Class2
                { }

                public interface Interface1
                { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new MethodId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Method1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<MethodId>(docs.MemberId);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.Exceptions);
            Assert.Equal(2, docs.Exceptions.Count);
            Assert.Contains(docs.Exceptions, x => x.Type.Equals(MemberId.Parse("T:System.InvalidOperationException", Array.Empty<TypeId>())));
            Assert.Contains(docs.Exceptions, x => x.Type.Equals(MemberId.Parse("T:System.ArgumentException", Array.Empty<TypeId>())));

            Assert.NotNull(docs.Parameters);
            Assert.Single(docs.Parameters);
            Assert.Contains(docs.Parameters, x => x.Key == "parameter1");

            Assert.NotNull(docs.Returns);

            Assert.All(docs.SeeAlso, seeAlso => Assert.NotNull(seeAlso.MemberId));

            Assert.Equal(2, docs.SeeAlso.Count);
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 1 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"))
            );
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 0 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"))
            );
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_field()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    /// <summary>
                    /// Summary
                    /// </summary>
                    /// <remarks>Remarks</remarks>
                    /// <example>Example</example>
                    /// <value>Value</value>
                    /// <seealso cref=""Class2"">Some text</seealso>
                    /// <seealso cref=""Interface1"" />
                    public string Field1;
                }

                public class Class2
                { }

                public interface Interface1
                { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new FieldId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Field1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<FieldId>(docs.MemberId);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.Value);

            Assert.All(docs.SeeAlso, seeAlso => Assert.NotNull(seeAlso.MemberId));

            Assert.Equal(2, docs.SeeAlso.Count);
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 1 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"))
            );
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 0 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"))
            );
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_property()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    /// <summary>
                    /// Summary
                    /// </summary>
                    /// <remarks>Remarks</remarks>
                    /// <example>Example</example>
                    /// <value>Value</value>
                    /// <exception cref=""ArgumentException"">Exception 1</exception>
                    /// <seealso  cref=""Class2"">Some text</seealso>
                    /// <seealso  cref=""Interface1"" />
                    public string Property1 { get; }
                }

                public class Class2
                { }

                public interface Interface1
                { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new PropertyId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Property1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<PropertyId>(docs.MemberId);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.Value);

            Assert.NotNull(docs.Exceptions);
            Assert.Single(docs.Exceptions);
            Assert.Contains(docs.Exceptions, x => x.Type.Equals(MemberId.Parse("T:System.ArgumentException", Array.Empty<TypeId>())));

            Assert.All(docs.SeeAlso, seeAlso => Assert.NotNull(seeAlso.MemberId));

            Assert.Equal(2, docs.SeeAlso.Count);
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 1 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"))
            );
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 0 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"))
            );
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_event()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    /// <summary>
                    /// Summary
                    /// </summary>
                    /// <remarks>Remarks</remarks>
                    /// <example>Example</example>
                    /// <seealso  cref=""Class2"">Some text</seealso>
                    /// <seealso  cref=""Interface1"" />
                    public event EventHandler Event1;
                }

                public class Class2
                { }

                public interface Interface1
                { }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new EventId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "Event1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<EventId>(docs.MemberId);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);


            Assert.Equal(2, docs.SeeAlso.Count);
            Assert.All(docs.SeeAlso, seeAlso => Assert.NotNull(seeAlso.MemberId));
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 1 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"))
            );
            Assert.Contains(
                docs.SeeAlso,
                seeAlso => seeAlso.Text.Elements.Count == 0 && seeAlso.MemberId!.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"))
            );
        }

        [Fact]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_nested_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                        /// <summary>
                    /// Example of an inner class
                    /// </summary>
                    public class NestedClass1
                    { }
                }
            ";

            using var assembly = Compile(cs, out var xmlDocs);

            var id = new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"), "NestedClass1");

            // ACT
            var sut = new XmlDocsProvider(assembly, xmlDocs, NullLogger.Instance);
            var docs = sut.TryGetDocumentationComments(id);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs!.MemberId);
            Assert.IsAssignableFrom<TypeId>(docs.MemberId);

            Assert.NotNull(docs.Summary);
        }
    }
}
