using System;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ExceptionElement
    {
        private readonly NuDoq.Exception m_NuDoqModel;

        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }

        public TextBlock Text { get; }


        public ExceptionElement(NuDoq.Exception nuDoqModel, TextBlock text)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }        
    }
}
