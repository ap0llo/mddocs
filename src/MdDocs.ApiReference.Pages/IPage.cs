using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public interface IPage : IMarkdownDocument
    {
        bool TryGetAnchor(MemberId id, out string anchor);
    }
}
