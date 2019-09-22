using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public interface IPage : IDocument
    {
        OutputPath OutputPath { get; }

        bool TryGetAnchor(MemberId id, out string anchor);

        void Save();
    }
}
