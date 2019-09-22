namespace Grynwald.MdDocs.ApiReference.Pages
{
    public interface IApiReferencePathProvider
    {
        string GetPath(ConstructorsPage page);

        string GetPath(EventPage page);

        string GetPath(FieldPage page);

        string GetPath(IndexerPage page);

        string GetPath(MethodPage page);

        string GetPath(NamespacePage page);

        string GetPath(OperatorPage page);

        string GetPath(PropertyPage page);

        string GetPath(TypePage page);
    }
}
