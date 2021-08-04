namespace Grynwald.MdDocs.ApiReference.Model
{
    public class DuplicateItemException : InvalidModelException
    {
        public DuplicateItemException(string message) : base(message)
        { }
    }
}
