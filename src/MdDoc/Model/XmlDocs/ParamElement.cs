using System;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ParamElement
    {
        private readonly Param m_NuDoqModel;

        public string Name => m_NuDoqModel.Name;

        public TextBlock Text { get; }

        public ParamElement(Param nuDoqModel, TextBlock text)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }        
    }
}
