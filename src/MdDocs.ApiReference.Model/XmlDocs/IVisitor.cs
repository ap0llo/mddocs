namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public interface IVisitor
    {
        void Visit(ParamRefElement element);

        void Visit(TypeParamRefElement element);

        void Visit(CElement element);

        void Visit(CodeElement element);

        void Visit(TextElement element);

        void Visit(SeeElement element);

        void Visit(TextBlock textBlock);

        void Visit(ParaElement element);
    }
}
