using Grynwald.MarkdownGenerator;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Pages
{
    class XmlDocToMarkdownConverter
    {
        class ConvertToBlockVisitor : Visitor
        {
            private MdCompositeSpan m_CurrentParagraph = new MdCompositeSpan();

            public MdContainerBlock Result { get; } = new MdContainerBlock();


            public override void VisitSummary(Summary summary)
            {
                PushParagraph();                
                base.VisitSummary(summary);
            }

            public override void VisitText(Text text)
            {
                m_CurrentParagraph.Add(new MdTextSpan(text.Content));
            }

            public override void VisitCode(Code code)
            {
                PushParagraph();
                Result.Add(new MdCodeBlock(code.Content));
            }


            private void PushParagraph()
            {
                if(m_CurrentParagraph.Spans.Count > 0)
                {
                    Result.Add(new MdParagraph(m_CurrentParagraph));
                }
                m_CurrentParagraph = new MdCompositeSpan();

            }
        }

        class ConvertToSpanVisitor : Visitor
        {
            public MdCompositeSpan Result { get; } = new MdCompositeSpan();


            public override void VisitText(Text text)
            {
                Result.Add(new MdTextSpan(text.Content));
            }

        }

        public static MdBlock ConvertToBlock(Summary summary)
        {
            var visitor = new ConvertToBlockVisitor();
            summary.Accept(visitor);

            return visitor.Result;

        }

        public static MdSpan ConvertToSpan(Summary summary)
        {
            var visitor = new ConvertToSpanVisitor();
            summary.Accept(visitor);

            return visitor.Result;
        }

    }    
}
