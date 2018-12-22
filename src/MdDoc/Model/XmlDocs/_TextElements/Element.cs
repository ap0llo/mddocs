namespace MdDoc.Model.XmlDocs
{
    public abstract class Element
    {
        public abstract TResult Accept<TResult, TParameter>(IVisitor<TResult, TParameter> visitor, TParameter parameter);
    }
}
