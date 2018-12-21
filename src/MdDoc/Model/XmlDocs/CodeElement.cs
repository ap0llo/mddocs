using System;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class CodeElement : Element
    {
        private readonly Code m_Code;


        public string Content => m_Code.Content;


        internal CodeElement(Code code)
        {
            m_Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
