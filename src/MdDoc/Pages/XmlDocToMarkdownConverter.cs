using System;
using Grynwald.MarkdownGenerator;
using MdDoc.Model.XmlDocs;

namespace MdDoc.Pages
{
    //TODO: This needs cleanup
    class XmlDocToMarkdownConverter
    {
        class ConvertToBlockVisitor : IVisitor<object, object>
        {
            private MdCompositeSpan m_CurrentParagraph = new MdCompositeSpan();
            private IMdSpanFactory m_SpanFactory;

            public MdContainerBlock Result { get; } = new MdContainerBlock();

            public ConvertToBlockVisitor(IMdSpanFactory spanFactory)
            {
                m_SpanFactory = spanFactory ?? throw new System.ArgumentNullException(nameof(spanFactory));
            }


            public object Visit(ParamRefElement element, object parameter)
            {
                return null;
            }

            public object Visit(TypeParamRefElement element, object parameter)
            {
                return null;
            }

            public object Visit(CElement element, object parameter)
            {
                if (!String.IsNullOrEmpty(element.Content))
                {
                    m_CurrentParagraph.Add(new MdCodeSpan(element.Content));
                }
                return null;
            }

            public object Visit(CodeElement element, object parameter)
            {
                PushParagraph();
                Result.Add(new MdCodeBlock(element.Content));
                return null;
            }

            public object Visit(TextElement element, object parameter)
            {
                m_CurrentParagraph.Add(new MdTextSpan(element.Content));
                return null;
            }

            public object Visit(SeeElement element, object parameter)
            {
                m_CurrentParagraph.Add(m_SpanFactory.GetMdSpan(element.MemberId));
                return null;
            }

            public object Visit(TextBlock textBlock, object parameter)
            {
                // end previous paragraph
                PushParagraph();

                foreach (var child in textBlock.Elements)
                {
                    child.Accept(this, parameter);
                }

                PushParagraph();
                return null;
            }            

            public object Visit(ParaElement element, object parameter)
            {
                return Visit(element.Text, parameter);
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

        class ConvertToSpanVisitor : IVisitor<object, object>
        {
            private IMdSpanFactory m_SpanFactory;


            public MdCompositeSpan Result { get; } = new MdCompositeSpan();


            public ConvertToSpanVisitor(IMdSpanFactory spanFactory)
            {
                m_SpanFactory = spanFactory ?? throw new System.ArgumentNullException(nameof(spanFactory));
            }


            public object Visit(ParamRefElement element, object parameter)
            {
                return null;
            }

            public object Visit(TypeParamRefElement element, object parameter)
            {
                return null;
            }

            public object Visit(CElement element, object parameter)
            {
                if (!String.IsNullOrEmpty(element.Content))
                {
                    Result.Add(new MdCodeSpan(element.Content));
                }
                return null;
            }

            public object Visit(CodeElement element, object parameter)
            {
                return null;
            }

            public object Visit(TextElement element, object parameter)
            {
                Result.Add(new MdTextSpan(element.Content));
                return null;
            }

            //public object Visit(SeeAlsoElement seeAlso, object parameter)
            //{
            //    if(seeAlso.Text.Elements.Count > 0)
            //    {
            //        var visitor = new ConvertToSpanVisitor(m_SpanFactory);
            //        foreach(var element in seeAlso.Elements)
            //        {
            //            element.Accept(visitor, null);
            //        }

            //        Result.Add(m_SpanFactory.CreateLink(seeAlso.MemberId, visitor.Result));
            //    }
            //    else
            //    {
            //        Result.Add(m_SpanFactory.GetMdSpan(seeAlso.MemberId));
            //    }

            //    return null;
            //}

            public object Visit(SeeElement element, object parameter)
            {
                Result.Add(m_SpanFactory.GetMdSpan(element.MemberId));
                return null;
            }

            public object Visit(TextBlock text, object parameter)
            {
                foreach(var child in  text.Elements)
                {
                    child.Accept(this, parameter);
                }

                return null;
            }            

            public object Visit(ParaElement element, object parameter)
            {
                return null;
            }
        }



        public static MdBlock ConvertToBlock(TextBlock text, IMdSpanFactory spanFactory)
        {
            var visitor = new ConvertToBlockVisitor(spanFactory);
            text.Accept(visitor, null);

            return visitor.Result;
        }

        
        public static MdSpan ConvertToSpan(TextBlock text, IMdSpanFactory spanFactory)
        {
            var visitor = new ConvertToSpanVisitor(spanFactory);
            text.Accept(visitor, null);

            return visitor.Result;
        }
    }    
}
