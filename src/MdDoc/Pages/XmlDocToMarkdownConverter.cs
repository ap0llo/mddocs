using Grynwald.MarkdownGenerator;
using MdDoc.Model.XmlDocs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Pages
{
    class XmlDocToMarkdownConverter
    {
        class ConvertToBlockVisitor : IVisitor<object, object>
        {
            private MdCompositeSpan m_CurrentParagraph = new MdCompositeSpan();

            public MdContainerBlock Result { get; } = new MdContainerBlock();

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

            public object Visit(SeeAlsoElement element, object parameter)
            {
                return null;
            }

            public object Visit(SeeElement element, object parameter)
            {
                return null;
            }

            public object Visit(SummaryElement element, object parameter)
            {
                // end previous paragraph
                PushParagraph();

                foreach (var child in element.Elements)
                {
                    child.Accept(this, parameter);
                }

                PushParagraph();
                return null;
            }

            public object Visit(ExampleElement element, object parameter)
            {
                return null;
            }

            public object Visit(RemarksElement element, object parameter)
            {
                return null;
            }

            public object Visit(ExceptionElement element, object parameter)
            {
                return null;
            }

            public object Visit(ParaElement element, object parameter)
            {
                return null;
            }

            public object Visit(TypeParamElement element, object parameter)
            {
                return null;
            }

            public object Visit(ParamElement element, object parameter)
            {
                return null;
            }

            public object Visit(ValueElement element, object parameter)
            {
                return null;
            }

            public object Visit(ReturnsElement element, object parameter)
            {
                return null;
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
            public MdCompositeSpan Result { get; } = new MdCompositeSpan();

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

            public object Visit(SeeAlsoElement element, object parameter)
            {
                return null;
            }

            public object Visit(SeeElement element, object parameter)
            {
                return null;
            }

            public object Visit(SummaryElement element, object parameter)
            {
                foreach(var child in  element.Elements)
                {
                    child.Accept(this, parameter);
                }

                return null;
            }

            public object Visit(ExampleElement element, object parameter)
            {
                return null;
            }

            public object Visit(RemarksElement element, object parameter)
            {
                return null;
            }

            public object Visit(ExceptionElement element, object parameter)
            {
                return null;
            }

            public object Visit(ParaElement element, object parameter)
            {
                return null;
            }

            public object Visit(TypeParamElement element, object parameter)
            {
                return null;
            }

            public object Visit(ParamElement element, object parameter)
            {
                return null;
            }

            public object Visit(ValueElement element, object parameter)
            {
                return null;
            }

            public object Visit(ReturnsElement element, object parameter)
            {
                return null;
            }
        }

        public static MdBlock ConvertToBlock(SummaryElement summary)
        {
            var visitor = new ConvertToBlockVisitor();
            summary.Accept(visitor, null);

            return visitor.Result;

        }

        public static MdSpan ConvertToSpan(SummaryElement summary)
        {
            var visitor = new ConvertToSpanVisitor();
            summary.Accept(visitor, null);

            return visitor.Result;
        }

    }    
}
