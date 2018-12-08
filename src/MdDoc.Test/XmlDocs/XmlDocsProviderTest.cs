using MdDoc.Test.TestData;
using MdDoc.XmlDocs;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace MdDoc.Test.XmlDocs
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

            XDocument.Load(m_XmlDocsPath);


        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation))]
        public void TryGetSummary_returns_null_for_type_without_summary(Type type)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(typeDefinition);

            // ASSERT
            Assert.Null(summary);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Method1))]
        public void TryGetSummary_returns_null_for_method_without_summary(Type type, string methodName)
        {
            // ARRANGE
            var methodDefinition = GetTypeDefinition(type)
                .Methods
                .Single(x => x.Name == methodName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(methodDefinition);

            // ASSERT
            Assert.Null(summary);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Field1))]
        public void TryGetSummary_returns_null_for_field_without_summary(Type type, string fieldName)
        {
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(type)
                .Fields
                .Single(x => x.Name == fieldName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(fieldDefinition);

            // ASSERT
            Assert.Null(summary);
        }


        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Property1))]
        public void TryGetSummary_returns_null_for_property_without_summary(Type type, string propertyName)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(type)
                .Properties
                .Single(x => x.Name == propertyName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(propertyDefinition);

            // ASSERT
            Assert.Null(summary);
        }

        [Theory]
        [InlineData(typeof(TestClass_NoDocumentation), nameof(TestClass_NoDocumentation.Event1))]
        public void TryGetSummary_returns_null_for_event_without_summary(Type type, string eventName)
        {
            // ARRANGE
            var eventDefinition = GetTypeDefinition(type)
                .Events
                .Single(x => x.Name == eventName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(eventDefinition);

            // ASSERT
            Assert.Null(summary);
        }

        [Theory]
        [InlineData(typeof(TestClass_Type))]
        public void TryGetSummary_gets_summary_for_type(Type type)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(typeDefinition);

            // ASSERT
            Assert.NotNull(summary);
        }


        [Theory]
        [InlineData(typeof(TestClass_Methods), nameof(TestClass_Methods.TestMethod1))]
        public void TryGetSummary_gets_summary_for_a_method(Type type, string methodName)
        {
            // ARRANGE
            var methodDefinition = GetTypeDefinition(type)
                .Methods
                .Single(x => x.Name == methodName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(methodDefinition);

            // ASSERT
            Assert.NotNull(summary);
        }

        [Theory]
        [InlineData(typeof(TestClass_Fields), nameof(TestClass_Fields.Field1))]
        public void TryGetSummary_gets_summary_for_a_field(Type type, string fieldName)
        {
            // ARRANGE
            // ARRANGE
            var fieldDefinition = GetTypeDefinition(type)
                .Fields
                .Single(x => x.Name == fieldName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(fieldDefinition);

            // ASSERT
            Assert.NotNull(summary);
        }        

        [Theory]
        [InlineData(typeof(TestClass_Properties), nameof(TestClass_Properties.Property1))]
        public void TryGetSummary_gets_summary_for_a_property(Type type, string propertyName)
        {
            // ARRANGE
            var propertyDefinition = GetTypeDefinition(type)
                .Properties
                .Single(x => x.Name == propertyName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(propertyDefinition);

            // ASSERT
            Assert.NotNull(summary);
        }


        [Theory]
        [InlineData(typeof(TestClass_Events), nameof(TestClass_Events.Event1))]
        public void TryGetSummary_gets_summary_for_a_event(Type type, string eventName)
        {
            // ARRANGE
            var eventDefinition = GetTypeDefinition(type)
                .Events
                .Single(x => x.Name == eventName);

            // ACT
            var sut = new XmlDocsProvider(m_XmlDocsPath);
            var summary = sut.TryGetSummary(eventDefinition);

            // ASSERT
            Assert.NotNull(summary);
        }
    }
}
