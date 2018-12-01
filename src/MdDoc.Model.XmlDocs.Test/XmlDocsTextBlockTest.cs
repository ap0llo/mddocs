using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Model.XmlDocs.Test
{
    public class XmlDocsTextBlockTest : TestBase
    {
        [Theory]        
        [InlineData("TestMethod_Summary_01", "Line1")]
        [InlineData("TestMethod_Summary_02", "Line1\r\nLine2")]
        [InlineData("TestMethod_Summary_03", "Line1\r\n\r\nLine2")]
        public void Text_returns_the_expected_value(string memberName, string expectedText)
        {
            var testData = GetMember($"M:MdDoc.Test.TestData.TestClass_XmlDocs.{memberName}").Element("summary");

            var sut = new XmlDocsTextBlock(testData);

            Assert.NotNull(sut.Text);
            Assert.Equal(expectedText, sut.Text);
        }
    }
}
