using System;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class TypeParamElement
    {
        private readonly TypeParam m_NuDoqModel;


        public string Name => m_NuDoqModel.Name;

        public TextBlock Text { get; }

        public TypeParamElement(TypeParam nuDoqModel, TextBlock text)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }        
    }
}
