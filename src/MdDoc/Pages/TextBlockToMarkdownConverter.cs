using System;
using Grynwald.MarkdownGenerator;
using MdDoc.Model.XmlDocs;

namespace MdDoc.Pages
{
    //TODO: This needs cleanup
    class TextBlockToMarkdownConverter
    {
        class ConvertToBlockVisitor : IVisitor
        {
            private MdCompositeSpan m_CurrentParagraph = new MdCompositeSpan();
            private IMdSpanFactory m_SpanFactory;

            public MdContainerBlock Result { get; } = new MdContainerBlock();


            public ConvertToBlockVisitor(IMdSpanFactory spanFactory)
            {
                m_SpanFactory = spanFactory ?? throw new System.ArgumentNullException(nameof(spanFactory));
            }


            public void Visit(ParamRefElement element)
            {
                m_CurrentParagraph.Add(new MdCodeSpan(element.Name));                
            }

            public void Visit(TypeParamRefElement element)
            {
                m_CurrentParagraph.Add(new MdCodeSpan(element.Name));
            }

            public void Visit(CElement element)
            {
                if (!String.IsNullOrEmpty(element.Content))
                {
                    m_CurrentParagraph.Add(new MdCodeSpan(element.Content));
                }
            }

            public void Visit(CodeElement element)
            {
                PushParagraph();
                Result.Add(new MdCodeBlock(element.Content));
            }

            public void Visit(TextElement element)
            {
                m_CurrentParagraph.Add(new MdTextSpan(element.Content));
            }

            public void Visit(SeeElement element)
            {
                m_CurrentParagraph.Add(m_SpanFactory.GetMdSpan(element.MemberId));
            }

            public void Visit(TextBlock textBlock)
            {
                // end previous paragraph
                PushParagraph();

                foreach (var child in textBlock.Elements)
                {
                    child.Accept(this);
                }

                PushParagraph();
            }            

            public void Visit(ParaElement element)
            {
                Visit(element.Text);
            }
            
            
            private void PushParagraph()
            {
                if (m_CurrentParagraph.Spans.Count > 0)
                {
                    Result.Add(new MdParagraph(m_CurrentParagraph));
                }
                m_CurrentParagraph = new MdCompositeSpan();
            }            
        }

        class ConvertToSpanVisitor : IVisitor
        {
            private IMdSpanFactory m_SpanFactory;


            public MdCompositeSpan Result { get; } = new MdCompositeSpan();


            public ConvertToSpanVisitor(IMdSpanFactory spanFactory)
            {
                m_SpanFactory = spanFactory ?? throw new System.ArgumentNullException(nameof(spanFactory));
            }


            public void Visit(ParamRefElement element)
            {
                Result.Add(new MdCodeSpan(element.Name));
            }

            public void Visit(TypeParamRefElement element)
            {
                Result.Add(new MdCodeSpan(element.Name));
            }

            public void Visit(CElement element)
            {
                if (!String.IsNullOrEmpty(element.Content))
                {
                    Result.Add(new MdCodeSpan(element.Content));
                }
            }

            public void Visit(CodeElement element)
            {
                // <code></code> cannot be converted to a span => ignore element
            }

            public void Visit(TextElement element)
            {
                Result.Add(new MdTextSpan(element.Content));
            }

            public void Visit(SeeElement element)
            {
                Result.Add(m_SpanFactory.GetMdSpan(element.MemberId));
            }

            public void Visit(TextBlock text)
            {
                foreach(var child in  text.Elements)
                {
                    child.Accept(this);
                }
            }

            public void Visit(ParaElement element)
            {
                // a single span cannot contain multplie paragraphs, but we can at least add a line break
                Result.Add(new MdTextSpan("\r\n"));
            }
        }



        public static MdBlock ConvertToBlock(TextBlock text, IMdSpanFactory spanFactory)
        {
            var visitor = new ConvertToBlockVisitor(spanFactory);
            text.Accept(visitor);

            return visitor.Result;
        }

        
        public static MdSpan ConvertToSpan(TextBlock text, IMdSpanFactory spanFactory)
        {
            var visitor = new ConvertToSpanVisitor(spanFactory);
            text.Accept(visitor);

            return visitor.Result;
        }
    }    
}
