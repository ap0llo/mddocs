using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace MdDoc.Model.XmlDocs.Test
{
    public class XmlDocsMemberTest : TestBase
    {
        [Fact]
        public void Constructor_throws_Exception_if_element_name_is_not_member()
        {
            var element = new XElement("someName");
            Assert.Throws<InvalidXmlDocsException>(() => new XmlDocsMember(element));
        }

        [Fact]
        public void Constructor_throws_Exception_if_name_attribute_is_missing()
        {
            var element = new XElement("member");
            Assert.Throws<InvalidXmlDocsException>(() => new XmlDocsMember(element));
        }

        [Fact]
        public void Constructor_throws_Exception_if_name_is_empty()
        {
            var element = new XElement("member", new XAttribute("name", ""));
            Assert.Throws<InvalidXmlDocsException>(() => new XmlDocsMember(element));
        }

        [Fact]
        public void Name_returns_the_expected_value()
        {
            var element = new XElement("member", new XAttribute("name", "SomeName"));
            var member = new XmlDocsMember(element);

            Assert.Equal("SomeName", member.Name);
        }

        [Fact]
        public void A_members_summary_is_loaded()
        {
            var testData = GetMember("T:MdDoc.Test.TestData.TestClass_XmlDocs");

            var sut = new XmlDocsMember(testData);
            
            Assert.NotNull(sut.Summary);            
        }
    }
}
