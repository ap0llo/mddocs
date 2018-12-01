using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace MdDoc.Model.XmlDocs.Test
{
    public class XmlDocsDocumentTest
    {

        [Fact]
        public void Constructor_throws_Exception_if_root_element_name_is_not_doc()
        {
            var document= new XDocument(new XElement("someName"));
            Assert.Throws<InvalidXmlDocsException>(() => new XmlDocsDocument(document));
        }


        [Fact]
        public void Members_returns_the_expected_elements()
        {
            var document = new XDocument(
                new XElement("doc", 
                    new XElement("members",
                        new XElement("member", new XAttribute("name", "Member1")),
                        new XElement("member", new XAttribute("name", "Member2"))
            )));
        
            var xmlDocs = new XmlDocsDocument(document);

            Assert.NotNull(xmlDocs.Members);
            Assert.Equal(2, xmlDocs.Members.Count);
            Assert.Contains(xmlDocs.Members, x => x.Name == "Member1");
            Assert.Contains(xmlDocs.Members, x => x.Name == "Member2");
        }

        [Fact]
        public void Members_can_be_empty()
        {
            var document = new XDocument(
                new XElement("doc")
            );

            var xmlDocs = new XmlDocsDocument(document);

            Assert.NotNull(xmlDocs.Members);
            Assert.Empty(xmlDocs.Members);            
        }
    }
}
