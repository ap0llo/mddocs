namespace Grynwald.MdDocs.ApiReference.Pages
{
    public interface IPage
    {
        OutputPath OutputPath { get; }

        void Save();
    }
}
