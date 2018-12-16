using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Model.XmlDocs.Test
{
    public class XmlDocsTextBlockTest : TestBase
    {
        private const string s_ClassName = "MdDoc.Test.TestData.TestClass_XmlDocs`2";

        [Theory]        
        [InlineData("TestMethod01(System.String)", "Line1\r\n")]
        [InlineData("TestMethod02", "Line1\r\nLine2\r\n")]
        [InlineData("TestMethod03", "Line1\r\n\r\nLine2\r\n")]
        public void Elements_returns_the_expected_items_01(string memberName, string expectedText)
        {
            var testData = GetMember($"M:{s_ClassName}.{memberName}").Element("summary");

            var sut = new XmlDocsTextBlock(testData);

            Assert.NotNull(sut.Elements);
            Assert.Single(sut.Elements);
            Assert.Equal(expectedText, (sut.Elements.Single() as PlainTextElement).Text);
        }


        [Theory] 
        [InlineData("TestMethod04")]
        public void Elements_returns_the_expected_items_02(string memberName)
        {
            var testData = GetMember($"M:{s_ClassName}.{memberName}").Element("summary");

            var sut = new XmlDocsTextBlock(testData);

            Assert.NotNull(sut.Elements);
            Assert.Equal(3, sut.Elements.Count);
            Assert.Equal("Line1\r\n", (sut.Elements[0] as PlainTextElement).Text);
            Assert.Equal("T:MdDoc.Test.TestData.TestClass_Events", (sut.Elements[1] as SeeElement).Cref);
            Assert.Equal("\r\nLine2\r\n", (sut.Elements[2] as PlainTextElement).Text);
        }

        [Theory]
        [InlineData("TestMethod05")]
        public void Elements_returns_the_expected_items_03(string memberName)
        {
            var testData = GetMember($"M:{s_ClassName}.{memberName}").Element("summary");

            var sut = new XmlDocsTextBlock(testData);

            Assert.NotNull(sut.Elements);
            Assert.Equal(3, sut.Elements.Count);
            Assert.Equal("Content ", (sut.Elements[0] as PlainTextElement).Text);
            Assert.Equal("T:MdDoc.Test.TestData.TestClass_Events", (sut.Elements[1] as SeeElement).Cref);
            Assert.Equal(" Content\r\n", (sut.Elements[2] as PlainTextElement).Text);
        }
    }
}
