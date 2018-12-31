using System;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ExceptionElement
    {
        public MemberId MemberId { get; }

        public TextBlock Text { get; }


        public ExceptionElement(MemberId memberId, TextBlock text)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
                
    }
}
