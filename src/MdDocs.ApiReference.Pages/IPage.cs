using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public interface IPage : IDocument
    {
        bool TryGetAnchor(MemberId id, out string anchor);
    }
}
