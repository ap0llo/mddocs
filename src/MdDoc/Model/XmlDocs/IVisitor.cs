namespace MdDoc.Model.XmlDocs
{
    public interface IVisitor<TResult, TParameter>
    {        
        TResult Visit(ParamRefElement element, TParameter parameter);

        TResult Visit(TypeParamRefElement element, TParameter parameter);

        TResult Visit(CElement element, TParameter parameter);

        TResult Visit(CodeElement element, TParameter parameter);

        TResult Visit(TextElement element, TParameter parameter);
       
        TResult Visit(SeeElement element, TParameter parameter);

        TResult Visit(TextBlock textBlock, TParameter parameter);
        
        TResult Visit(ParaElement element, TParameter parameter);               
    }
}
