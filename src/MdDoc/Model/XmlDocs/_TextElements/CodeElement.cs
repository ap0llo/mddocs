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

        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
