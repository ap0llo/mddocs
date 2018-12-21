using System;

namespace MdDoc.Model.XmlDocs
{
    public sealed class TypeParamRefElement : Element
    {
        private readonly NuDoq.TypeParamRef m_NuDoqModel;


        public string Name => m_NuDoqModel.Name;


        public TypeParamRefElement(NuDoq.TypeParamRef nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
        }


        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);
    }
}
