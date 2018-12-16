using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public interface IVisitor<TResult, TParameter>
    {        
        TResult Visit(ParamRefElement element, TParameter parameter);

        TResult Visit(TypeParamRefElement element, TParameter parameter);

        TResult Visit(CElement element, TParameter parameter);

        TResult Visit(CodeElement element, TParameter parameter);

        TResult Visit(TextElement element, TParameter parameter);

        TResult Visit(SeeAlsoElement element, TParameter parameter);

        TResult Visit(SeeElement element, TParameter parameter);

        TResult Visit(SummaryElement element, TParameter parameter);

        TResult Visit(ExampleElement element, TParameter parameter);

        TResult Visit(RemarksElement element, TParameter parameter);

        TResult Visit(ExceptionElement element, TParameter parameter);

        TResult Visit(ParaElement element, TParameter parameter);

        TResult Visit(TypeParamElement element, TParameter parameter);

        TResult Visit(ParamElement element, TParameter parameter);

        TResult Visit(ValueElement element, TParameter parameter);

        TResult Visit(ReturnsElement element, TParameter parameter);
    }
}
