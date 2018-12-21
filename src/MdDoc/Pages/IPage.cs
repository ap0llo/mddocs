namespace MdDoc.Pages
{
    public interface IPage
    {
        OutputPath OutputPath { get; }

        void Save();
    }
}
