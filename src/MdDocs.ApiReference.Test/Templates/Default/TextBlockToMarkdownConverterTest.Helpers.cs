using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Xunit.Sdk;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    public partial class TextBlockToMarkdownConverterTest
    {
        private TextBlock CreateTextBlock(params Element[] elements) =>
            new TextBlock(elements);

        private TextBlock CreateTextBlock(string content) =>
            CreateTextBlock(new TextElement(content));

        private TextBlock CreateList(ListType listType, ListItemElement? header, params ListItemElement[] items) =>
            CreateTextBlock(new ListElement(listType, header, items));

        private TextBlock CreateList(ListType listType, ListItemElement? header, IEnumerable<ListItemElement> items) =>
            CreateTextBlock(new ListElement(listType, header, items.ToArray()));

        private ListItemElement CreateListItem(string text) =>
            new ListItemElement(null, CreateTextBlock(text));

        private ListItemElement CreateListItem(string term, string definition) =>
            new ListItemElement(CreateTextBlock(term), CreateTextBlock(definition));


        private void AssertEquals(MdBlock expected, MdBlock actual)
        {
            if (!expected.DeepEquals(actual))
            {
                throw new XunitException("MdBlock equality failure.\r\n" +
                    "Expected:\r\n" +
                    "\r\n" +
                    SyntaxVisualizer.GetSyntaxTree(expected) +
                    "\r\n" +
                    "\r\n" +
                    "Actual:\r\n" +
                    "\r\n" +
                    SyntaxVisualizer.GetSyntaxTree(actual)
                );
            }
        }
    }
}
