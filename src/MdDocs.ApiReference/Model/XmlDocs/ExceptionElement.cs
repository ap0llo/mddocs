using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public sealed class ExceptionElement
    {
        public TypeId Type { get; }

        public TextBlock Text { get; }


        public ExceptionElement(TypeId type, TextBlock text)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }                
    }
}
