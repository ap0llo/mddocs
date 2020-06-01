namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Visitor interface for traversing text elements
    /// </summary>
    /// <seealso cref="TextElement"/>
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

        void Visit(ListElement element);

        void Visit(ListItemElement element);
    }
}
