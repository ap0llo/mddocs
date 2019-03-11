using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    /// <summary>
    /// Visitor that converts a <see cref="TextBlock"/> to a markdown block (<see cref="MdBlock"/>).
    /// </summary>
    internal class ConvertToBlockVisitor : IVisitor
    {
        private readonly IMdSpanFactory m_SpanFactory;

        private readonly Stack<MdContainerBlockBase> m_Blocks;
        private MdParagraph m_CurrentParagraph;

        /// <summary>
        /// Gets the root block of the generated markdown
        /// </summary>
        public MdContainerBlock Result { get; }

        /// <summary>
        /// Gets the markdown block currently being appended to 
        /// </summary>
        private MdContainerBlockBase CurrentBlock => m_Blocks.Peek();


        public ConvertToBlockVisitor(IMdSpanFactory spanFactory)
        {
            m_SpanFactory = spanFactory ?? throw new ArgumentNullException(nameof(spanFactory));

            Result = new MdContainerBlock();

            m_Blocks = new Stack<MdContainerBlockBase>();
            m_Blocks.Push(Result);
        }


        public void Visit(ParamRefElement element) => AddToCurrentParagraph(new MdCodeSpan(element.Name));

        public void Visit(TypeParamRefElement element) => AddToCurrentParagraph(new MdCodeSpan(element.Name));

        public void Visit(CElement element)
        {
            if (!String.IsNullOrEmpty(element.Content))
                AddToCurrentParagraph(new MdCodeSpan(element.Content));
        }

        public void Visit(CodeElement element)
        {
            // end the current paragraph
            EndParagraph();

            // add a new code block to the current block
            CurrentBlock.Add(new MdCodeBlock(element.Content, element.Language));
        }

        public void Visit(TextElement element) => AddToCurrentParagraph(new MdTextSpan(element.Content));

        public void Visit(SeeElement element) => AddToCurrentParagraph(m_SpanFactory.GetMdSpan(element.MemberId));

        public void Visit(ParaElement element) => Visit(element.Text);

        public void Visit(TextBlock textBlock)
        {
            // begin a new paragraph
            EndParagraph();

            // visit text elements
            foreach (var element in textBlock.Elements)
            {
                element.Accept(this);
            }
        }

        public void Visit(ListElement listElement)
        {
            // end the current paragraph
            EndParagraph();

            if (listElement.Type == ListType.Table)
            {
                MdTableRow CreateRow(ListItemElement itemElement)
                {
                    var term = itemElement?.Term == null
                        ? MdEmptySpan.Instance
                        : TextBlockToMarkdownConverter.ConvertToSpan(itemElement.Term, m_SpanFactory);

                    var description = itemElement?.Description == null
                        ? MdEmptySpan.Instance
                        : TextBlockToMarkdownConverter.ConvertToSpan(itemElement.Description, m_SpanFactory);

                    return new MdTableRow(term, description);
                }

                var table = new MdTable(
                    CreateRow(listElement.ListHeader),
                    listElement.Items.Select(CreateRow)
                );

                CurrentBlock.Add(table);
            }
            else
            {

                var list = listElement.Type == ListType.Number ? new MdOrderedList() : (MdList)new MdBulletList();

                // add list to current block
                CurrentBlock.Add(list);

                foreach (var listItemElement in listElement.Items)
                {
                    // create a new list item and add it to the list
                    var listItem = new MdListItem();
                    list.Add(listItem);

                    // make the list item the new current block
                    m_Blocks.Push(listItem);

                    // visit list item
                    listItemElement.Accept(this);

                    // end the current paragraph and restore previous current block
                    EndParagraph();
                    m_Blocks.Pop();
                }
            }
        }

        public void Visit(ListItemElement element)
        {
            if (element.Term != null)
            {
                var term = TextBlockToMarkdownConverter.ConvertToSpan(element.Term, m_SpanFactory);
                
                AddToCurrentParagraph(new MdStrongEmphasisSpan(term, ":"));
                AddToCurrentParagraph(" ");                
            }
            
            Visit(element.Description);
        }

        /// <summary>
        /// Adds the specified span to the current paragraph.
        /// A new paragraph is created implicitly when there is no current paragraph.
        /// </summary>
        private void AddToCurrentParagraph(MdSpan span)
        {
            // create paragraph and add it to the current block
            if (m_CurrentParagraph == null)
            {
                m_CurrentParagraph = new MdParagraph();
                CurrentBlock.Add(m_CurrentParagraph);
            }

            m_CurrentParagraph.Add(span);
        }

        /// <summary>
        /// Ends the current paragraph
        /// </summary>
        private void EndParagraph()
        {
            m_CurrentParagraph = null;
        }
    }
}
