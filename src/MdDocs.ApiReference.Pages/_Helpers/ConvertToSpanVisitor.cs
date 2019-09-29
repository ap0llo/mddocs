using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    /// <summary>
    /// Visitor that converts a <see cref="TextBlock"/> to a markdown span (<see cref="MdSpan"/>).
    /// </summary>
    internal class ConvertToSpanVisitor : IVisitor
    {
        private readonly IMdSpanFactory m_SpanFactory;


        public MdCompositeSpan Result { get; } = new MdCompositeSpan();


        public ConvertToSpanVisitor(IMdSpanFactory spanFactory)
        {
            m_SpanFactory = spanFactory ?? throw new ArgumentNullException(nameof(spanFactory));
        }


        public void Visit(ParamRefElement element) => Result.Add(new MdCodeSpan(element.Name));

        public void Visit(TypeParamRefElement element) => Result.Add(new MdCodeSpan(element.Name));

        public void Visit(CElement element)
        {
            if (!String.IsNullOrEmpty(element.Content))
                Result.Add(new MdCodeSpan(element.Content));
        }

        public void Visit(CodeElement element)
        {
            // <code></code> cannot be converted to a span => ignore element
            //TODO: Log warning for ignored element
        }

        public void Visit(TextElement element) => Result.Add(new MdTextSpan(element.Content));

        public void Visit(SeeElement element)
        {
            MdSpan span;
            if(element.Text.IsEmpty)
            {
                span = m_SpanFactory.GetMdSpan(element.MemberId);
            }
            else
            {
                var linkText = TextBlockToMarkdownConverter.ConvertToSpan(element.Text, m_SpanFactory);
                span = m_SpanFactory.CreateLink(element.MemberId, linkText);
            }

            Result.Add(span);
        }

        public void Visit(TextBlock text)
        {
            foreach (var element in text.Elements)
            {
                element.Accept(this);
            }
        }

        public void Visit(ParaElement element)
        {
            // a single span cannot contain multiple paragraphs, but we can at least add a line break
            Result.Add("\r\n");

            // visit text block in paragraph
            element.Text.Accept(this);
        }

        public void Visit(ListElement element)
        {
            // Lists cannot be converted to a span => ignore element
            //TODO: Log warning for ignored element
        }

        public void Visit(ListItemElement element)
        {
            // Lists cannot be converted to a span => ignore element
            //TODO: Log warning for ignored element
        }
    }
}
