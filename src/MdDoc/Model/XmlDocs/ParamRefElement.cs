using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public sealed class ParamRefElement : Element
    {
        private readonly NuDoq.ParamRef m_NuDoqModel;


        public string Name => m_NuDoqModel.Name;


        public ParamRefElement(NuDoq.ParamRef nuDoqModel)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
        }

        public override TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter) => visitor.Visit(this, parameter);

    }
}
