using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal interface ILinkProvider
    {
        bool TryGetLink(MemberId id, out Link link);
    }
}
