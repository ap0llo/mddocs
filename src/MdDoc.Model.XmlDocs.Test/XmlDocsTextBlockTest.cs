using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Model.XmlDocs.Test
{
    public class XmlDocsTextBlockTest : TestBase
    {
        [Fact]
        public void Value_returns_the_expected_elements()
        {
            var testData = GetMember("T:MdDoc.Test.TestData.TestClass_XmlDocs").Element("summary");

            var sut = new XmlDocsTextBlock(testData);

            Assert.NotNull(sut.Text);
            Assert.Equal("This is a test class for testing parsing of XML documentation", sut.Text);
        }

    }
}
