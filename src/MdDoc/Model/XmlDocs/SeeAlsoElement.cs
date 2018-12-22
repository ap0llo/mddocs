using System;
using System.Collections.Generic;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public class SeeAlsoElement
    {
        private readonly SeeAlso m_NuDoqModel;

        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }

        public TextBlock Text { get; }


        public SeeAlsoElement(SeeAlso nuDoqModel, TextBlock text)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }
        
    }
}
