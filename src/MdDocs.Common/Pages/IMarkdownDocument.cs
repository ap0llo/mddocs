using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.Common.Pages
{
    public interface IMarkdownDocument : IDocument
    {
        void Save(string path, MdSerializationOptions markdownOptions);
    }
}
