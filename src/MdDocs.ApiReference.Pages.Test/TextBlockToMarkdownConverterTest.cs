using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Moq;
using Xunit;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages.Test
{
    public partial class TextBlockToMarkdownConverterTest
    {
        [Fact]
        public void ConvertToBlock_returns_expected_result_for_a_TextBlock()
        {
            // ARRANGE
            var input = CreateTextBlock(new TextElement("My Text"));
            var expected = new MdContainerBlock(new MdParagraph("My Text"));

            // ACT
            var actual = TextBlockToMarkdownConverter.ConvertToBlock(input, Mock.Of<IMdSpanFactory>());

            // ASSERT
            AssertEquals(expected, actual);
        }

        [Theory, CombinatorialData]
        public void ConvertToBlock_returns_expected_result_for_for_lists([CombinatorialValues(ListType.Bullet, ListType.None, ListType.Number)]ListType listType)
        {
            // ARRANGE
            var input = CreateList(
                listType,
                null,
                CreateListItem("Description 1"),
                CreateListItem("Term 2", "Description 2")
            );


            var list = listType == ListType.Number ? new MdOrderedList() : (MdList)new MdBulletList();
            list.Add(
                ListItem(
                    Paragraph("Description 1")));
            list.Add(
                ListItem(
                    Paragraph(StrongEmphasis("Term 2", ":"), " "),
                    Paragraph("Description 2")));

            var expected = Container(list);

            // ACT
            var actual = TextBlockToMarkdownConverter.ConvertToBlock(input, Mock.Of<IMdSpanFactory>());

            // ASSERT
            AssertEquals(expected, actual);
        }

        [Theory, CombinatorialData]
        public void ConvertToBlock_returns_expected_result_for_lists_02([CombinatorialValues(1, 2, 10)]int numberOfListItems)
        {
            // ARRANGE
            var input = CreateList(
                ListType.Number,
                null,
                Enumerable.Range(1, numberOfListItems).Select(i => CreateListItem($"Item {i}"))
            );

            var expected = Container(
                OrderedList(
                    Enumerable.Range(1, numberOfListItems).Select(i => ListItem($"Item {i}"))
            ));

            // ACT
            var actual = TextBlockToMarkdownConverter.ConvertToBlock(input, Mock.Of<IMdSpanFactory>());

            // ASSERT
            AssertEquals(expected, actual);
        }

        [Fact]
        public void ConvertToBlock_returns_expected_result_for_tables()
        {
            // ARRANGE
            var input = CreateList(
                ListType.Table,
                CreateListItem("Header 1", "Header 2"),
                CreateListItem("C1R1", "C2R1"),
                CreateListItem("C1R2", "C2R2")
            );

            var expected = Container(
                Table(
                    Row("Header 1", "Header 2"),
                    Row("C1R1", "C2R1"),
                    Row("C1R2", "C2R2")
            ));

            // ACT
            var actual = TextBlockToMarkdownConverter.ConvertToBlock(input, Mock.Of<IMdSpanFactory>());

            // ASSERT
            AssertEquals(expected, actual);
        }
    }
}
