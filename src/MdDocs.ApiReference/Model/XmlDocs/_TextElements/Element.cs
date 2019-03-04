namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public abstract class Element
    {
        public abstract void Accept(IVisitor visitor);
    }
}
