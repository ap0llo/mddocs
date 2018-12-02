using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public sealed class PlainTextElement : Element
    {
        public string Text { get; }


        public PlainTextElement(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
        
    }
}
