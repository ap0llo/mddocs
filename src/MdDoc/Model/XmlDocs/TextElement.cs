using NuDoq;
using System;

namespace MdDoc.Model.XmlDocs
{
    public class TextElement : Element
    {
        private readonly Text m_NuDoqModel;


        public string Content => m_NuDoqModel.Content;


        public TextElement(Text nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
