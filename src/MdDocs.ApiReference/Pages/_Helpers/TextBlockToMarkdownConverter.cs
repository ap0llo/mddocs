using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class TextBlockToMarkdownConverter
    {
        public static MdBlock ConvertToBlock(TextBlock text, IMdSpanFactory spanFactory)
        {
            if (text.IsEmpty)
            {
                return MdEmptyBlock.Instance;
            }

            var visitor = new ConvertToBlockVisitor(spanFactory);
            text.Accept(visitor);

            return visitor.Result;
        }

        public static MdSpan ConvertToSpan(TextBlock text, IMdSpanFactory spanFactory)
        {
            if (text.IsEmpty)
            {
                return MdEmptySpan.Instance;
            }

            var visitor = new ConvertToSpanVisitor(spanFactory);
            text.Accept(visitor);

            // flatten composite span if there is only a single item in it
            if (visitor.Result.Spans.Count == 1)
            {
                return visitor.Result.Spans[0];
            }

            return visitor.Result;
        }
    }
}
