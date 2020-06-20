using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public interface IPage : IMarkdownDocument
    {
        bool TryGetAnchor(MemberId id, out string? anchor);
    }
}
