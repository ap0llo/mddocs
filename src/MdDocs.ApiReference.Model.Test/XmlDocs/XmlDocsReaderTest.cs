using System;
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
            var xmlDocsReader = new XmlDocsReader(NullLogger.Instance);

            var members = xmlDocsReader.Read(document);
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

            Assert.NotNull(listItem.Term);

            if (expectTerm)
                Assert.False(listItem.Term.IsEmpty);
            else
                Assert.True(listItem.Term.IsEmpty);

            if (expectDescription)
                Assert.NotNull(listItem.Description);
            else
                Assert.Null(listItem.Description);
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_01()
        {
            var xml = @"            <para>
            Similar pages are also generated for interfaces, structs
            and enums
            </para> ";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new TextElement("Similar pages are also generated for interfaces, structs and enums")
            );
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_02()
        {
            var xml = @"            <para>
            <see cref=""T:DemoProject.DemoStruct"" />
            </para> ";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new SeeElement(MemberId.Parse("T:DemoProject.DemoStruct"))
            );
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_03()
        {
            var xml = @"            <para>
            Similar pages are also generated for interfaces, structs (see <see cref=""T:DemoProject.DemoStruct""/>)
            and enums
            </para> ";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new TextElement("Similar pages are also generated for interfaces, structs (see "),
                new SeeElement(MemberId.Parse("T:DemoProject.DemoStruct")),
                new TextElement(") and enums")
            );
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_04()
        {
            var xml = @"<para>Lorem ipsum dolor sit amet.</para>";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new TextElement("Lorem ipsum dolor sit amet.")
            );
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_05()
        {
            var xml = @"<para><see cref=""T:DemoProject.DemoClass"" /></para>";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new SeeElement(MemberId.Parse("T:DemoProject.DemoClass"))
            );
        }

        [Fact]
        public void ReadTextBlock_returns_the_expected_elements_06()
        {
            var xml = @"<para><see cref=""T:DemoProject.DemoClass"">Lorem ipsum dolor sit amet.</see></para>";

            ReadTextBlock_returns_the_expected_elements(
                xml,
                new SeeElement(
                    MemberId.Parse("T:DemoProject.DemoClass"),
                    new TextBlock(new[]
                    {
                        new TextElement("Lorem ipsum dolor sit amet.")
                    }))
            );
        }


        private void ReadTextBlock_returns_the_expected_elements(string xml, params Element[] expectedElements)
        {
            // ARRANGE
            var sut = new XmlDocsReader(NullLogger.Instance);
            var expected = new TextBlock(expectedElements);

            // ACT
            var actual = sut.ReadTextBlock(XElement.Parse(xml));

            // ASSERT            
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_01()
        {
            var xml = @"<seealso cref=""T:SomeNamespace.SomeClass"" />";

            var expected = new SeeAlsoElement(MemberId.Parse("T:SomeNamespace.SomeClass"));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }

        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_02()
        {
            var xml = @"<seealso cref=""T:SomeNamespace.SomeClass"">Lorem ipsum dolor sit ament.</seealso>";

            var expected = new SeeAlsoElement(
                MemberId.Parse("T:SomeNamespace.SomeClass"),
                new TextBlock(new[]
                {
                    new TextElement("Lorem ipsum dolor sit ament.")
                }));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }


        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_03()
        {
            var xml = @"<seealso href=""http://example.com"" />";

            var expected = new SeeAlsoElement(new Uri("http://example.com"));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }

        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_04()
        {
            var xml = @"<seealso href=""http://example.com"">Lorem ipsum dolor sit ament.</seealso>";

            var expected = new SeeAlsoElement(
                new Uri("http://example.com"),
                new TextBlock(new[]
                {
                    new TextElement("Lorem ipsum dolor sit ament.")
                }));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }


        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_05()
        {
            var xml = @"<seealso cref=""T:SomeNamespace.SomeClass"" href=""http://example.com"" />";

            var expected = new SeeAlsoElement(MemberId.Parse("T:SomeNamespace.SomeClass"));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }

        [Fact]
        public void ReadMemberContent_correctly_parses_seealso_elements_06()
        {
            var xml = @"<seealso cref=""T:SomeNamespace.SomeClass"" href=""http://example.com"">Lorem ipsum dolor sit ament.</seealso>";

            var expected = new SeeAlsoElement(
                MemberId.Parse("T:SomeNamespace.SomeClass"),
                new TextBlock(new[]
                {
                    new TextElement("Lorem ipsum dolor sit ament.")
                }));

            ReadMemberContent_correctly_parses_seealso_elements(xml, expected);
        }


        private void ReadMemberContent_correctly_parses_seealso_elements(string xml, SeeAlsoElement expected)
        {
            // ARRANGE
            xml = $@"<container>{xml}</container>";

            var sut = new XmlDocsReader(NullLogger.Instance);
            var memberElement = new MemberElement(MemberId.Parse("T:DemoProject.DemoClass"));

            // ACT
            sut.ReadMemberContent(XElement.Parse(xml), memberElement);

            // ASSERT
            var actual = Assert.Single(memberElement.SeeAlso);
            Assert.Equal(expected, actual);
        }
    }
}
