namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Interface for documentation items that can be marked as obsolete.
    /// </summary>
    public interface IObsoleteableDocumentation
    {
        bool IsObsolete { get; }

        string ObsoleteMessage { get; }
    }
}
