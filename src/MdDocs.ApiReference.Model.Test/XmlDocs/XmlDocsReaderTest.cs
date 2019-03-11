using System.Linq;
using System.Xml.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test.XmlDocs
{
    public class XmlDocsReaderTest
    {
        private static ListElement ReadList(XDocument document)
        {
            var members = XmlDocsReader.Read(document, NullLogger.Instance);
            Assert.Single(members);
            Assert.Single(members.Single().Summary.Elements);

            var element = members.Single().Summary.Elements.Single();
            Assert.IsType<ListElement>(element);

            var list = (ListElement)element;
            return list;
        }

        private static XDocument GetDocumentFromListXml(string listXml)
        {
            return XDocument.Parse($@"<?xml version=""1.0""?>
            <doc>
                <assembly>
                    <name>AssemblyName</name>
                </assembly>
                <members>
                    <member name=""T:TypeName"">
                        <summary>
                            {listXml}
                        </summary>
                    </member>
                </members>
            </doc>");
        }

        private static XDocument GetDocumentFromListItemXml(string listItemXml)
        {
            return GetDocumentFromListXml(
                $@"<list type=""bullet"">
                        <item>
                            {listItemXml}
                        </item>
                    </list>"
            );
        }


        [Theory]
        [InlineData(@"<list></list>", ListType.None, false, 0)]
        [InlineData(@"<list type=""bullet""></list>", ListType.Bullet, false, 0)]
        [InlineData(@"<list type=""number""></list>", ListType.Number, false, 0)]
        [InlineData(@"<list type=""table""></list>", ListType.Table, false, 0)]
        [InlineData(
            @"<list type=""bullet"">
                <item>
                    <description>description</description>  
                </item>
              </list>",
            ListType.Bullet, false, 1)]
        [InlineData(
            @"<list type=""bullet"">
                <item>
                    <description>description</description>  
                </item>
                <item>
                    <description>description</description>  
                </item>
              </list>",
            ListType.Bullet, false, 2)]
        [InlineData(
            @"<list type=""bullet"">
                <listheader>
                    <description>description</description>  
                </listheader>
                <item>
                    <description>description</description>  
                </item>
              </list>",
            ListType.Bullet, true, 1)]
        public void Lists_are_parsed_correctly(string xml, ListType expectedListType, bool expectHeader, int expectedNumberOfListItems)
        {
            // ARRANGE
            var document = GetDocumentFromListXml(xml);

            // ACT
            var list = ReadList(document);

            // ASSERT
            Assert.Equal(expectedListType, list.Type);

            if (expectHeader)
                Assert.NotNull(list.ListHeader);
            else
                Assert.Null(list.ListHeader);

            Assert.Equal(expectedNumberOfListItems, list.Items.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("<someElement></someElement>")]
        [InlineData("<term></term>")]
        public void Invalid_list_items_are_ignored(string xml)
        {
            // ARRANGE
            var document = GetDocumentFromListItemXml(xml);

            // ACT
            var list = ReadList(document);

            // ASSERT
            Assert.Empty(list.Items);
        }

        [Theory]
        [InlineData("<description>TestDescription</description>", false, true)]
        [InlineData("<term>TestTerm</term><description>TestDescription</description>", true, true)]
        public void List_items_are_parsed_correctly(string xml, bool expectTerm, bool expectDescription)
        {
            // ARRANGE
            var document = GetDocumentFromListItemXml(xml);

            // ACT
            var list = ReadList(document);

            // ASSERT
            Assert.Single(list.Items);

            var listItem = list.Items.Single();

            if (expectTerm)
                Assert.NotNull(listItem.Term);
            else
                Assert.Null(listItem.Term);

            if (expectDescription)
                Assert.NotNull(listItem.Description);
            else
                Assert.Null(listItem.Description);
        }

    }
}
