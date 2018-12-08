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
        public void TryGetSummary_gets_summary_for_method(Type type, string methodName)
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


    }
}
