using System;
using System.IO;
using System.Linq;
using MdDoc.Test.TestData;
using MdDoc.Model.XmlDocs;
using Xunit;

namespace MdDoc.Test.Model.XmlDocs
{
    public class XmlDocsProviderTest : TestBase
    {
        private readonly string m_XmlDocsPath;


        public XmlDocsProviderTest()
        {
            // This will load the XML Documentation files for the MdDoc.Test.TestData assembly
            // By default - this does not work with Visual Studio's Live Unit Testing
            // because it does not generate XML docs 
            // To enable this, go to "Tools" -> "Options" -> "Live Unit Testing" and
            // select "Enable debug symbol and xml documentation comment generation"
            // (see https://docs.microsoft.com/en-us/visualstudio/test/live-unit-testing?view=vs-2017#configure-live-unit-testing)

            var assemblyPath = new Uri(typeof(TestClass_Type).Assembly.CodeBase).LocalPath;
            m_XmlDocsPath = Path.ChangeExtension(assemblyPath, ".xml");            
        }


        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation))]
        public void TryGetDocumentationComments_returns_null_for_type_without_documentation(Type type)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(typeDefinition);

            // ASSERT
            Assert.Null(docs);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Method1))]
        public void TryGetDocumentationComments_returns_null_for_method_without_documentation(Type type, string methodName)
        {
            // ARRANGE
            var methodDefinition = GetTypeDefinition(type)
                .Methods
                .Single(x => x.Name == methodName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(methodDefinition);

            // ASSERT
            Assert.Null(docs);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Field1))]
        public void TryGetDocumentationComments_returns_null_for_field_without_documentation(Type type, string fieldName)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(type)
                .Fields
                .Single(x => x.Name == fieldName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(fieldDefinition);

            // ASSERT
            Assert.Null(docs);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Property1))]
        public void TryGetDocumentationComments_returns_null_for_property_without_documentation(Type type, string propertyName)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(type)
                .Properties
                .Single(x => x.Name == propertyName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(propertyDefinition);

            // ASSERT
            Assert.Null(docs);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Event1))]
        public void TryGetDocumentationComments_returns_null_for_event_without_documentation(Type type, string eventName)
        {
            // ARRANGE
            var eventDefinition = GetTypeDefinition(type)
                .Events
                .Single(x => x.Name == eventName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetDocumentationComments(eventDefinition);

            // ASSERT
            Assert.Null(summary);
        }


        [Theory]
        [InlineData(typeof(TestClass_XmlDocs<object,object>))]
        public void TryGetDocumentationComments_gets_expected_docs_summary_for_type(Type type)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(typeDefinition);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.TypeParameters);
            Assert.Equal(2, docs.TypeParameters.Count);
            Assert.Contains(docs.TypeParameters, x => x.Name == "T1");
            Assert.Contains(docs.TypeParameters, x => x.Name == "T2");
        }

        [Theory]
        [InlineData(typeof(TestClass_XmlDocs<,>), nameof(TestClass_XmlDocs<object, object>.TestMethod01))]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_method(Type type, string methodName)
        {
            // ARRANGE
            var methodDefinition = GetTypeDefinition(type)
                .Methods
                .Single(x => x.Name == methodName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(methodDefinition);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.Exceptions);
            Assert.Equal(2, docs.Exceptions.Count);
            Assert.Contains(docs.Exceptions, x => x.Cref == "T:System.InvalidOperationException");
            Assert.Contains(docs.Exceptions, x => x.Cref == "T:System.ArgumentException");
              

            Assert.NotNull(docs.Parameters);
            Assert.Single(docs.Parameters);
            Assert.Contains(docs.Parameters, x => x.Name == "parameter1");

            Assert.NotNull(docs.Returns);
        }

        [Theory]
        [InlineData(typeof(TestClass_XmlDocs<,>), nameof(TestClass_XmlDocs<object,object>.Field1))]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_field(Type type, string fieldName)
        {
            // ARRANGE
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(type)
                .Fields
                .Single(x => x.Name == fieldName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(fieldDefinition);

            // ASSERT
            Assert.NotNull(docs);
            Assert.NotNull(docs.Summary);
            Assert.NotNull(docs.Remarks);
            Assert.NotNull(docs.Example);
            Assert.NotNull(docs.Value);
        }

        [Theory]
        [InlineData(typeof(TestClass_XmlDocs<,>), nameof(TestClass_XmlDocs<object, object>.Property1))]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_property(Type type, string propertyName)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(type)
                .Properties
                .Single(x => x.Name == propertyName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(propertyDefinition);

            // ASSERT
            Assert.NotNull(docs);

            Assert.NotNull(docs.Summary);

            Assert.NotNull(docs.Remarks);

            Assert.NotNull(docs.Example);

            Assert.NotNull(docs.Value);

            Assert.NotNull(docs.Exceptions);
            Assert.Single(docs.Exceptions);
            Assert.Contains(docs.Exceptions, x => x.Cref == "T:System.ArgumentException");            
        }

        [Theory]
        [InlineData(typeof(TestClass_XmlDocs<,>), nameof(TestClass_XmlDocs<object, object>.Event1))]
        public void TryGetDocumentationComments_gets_expected_docs_for_a_event(Type type, string eventName)
        {
            // ARRANGE
            var eventDefinition = GetTypeDefinition(type)
                .Events
                .Single(x => x.Name == eventName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var docs = sut.TryGetDocumentationComments(eventDefinition);

            // ASSERT
            Assert.NotNull(docs);
            Assert.NotNull(docs.Summary);
            Assert.NotNull(docs.Remarks);
            Assert.NotNull(docs.Example);
        }
    }
}
