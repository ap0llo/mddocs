using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    interface ILinkProvider
    {
        bool TryGetLink(MemberId id, out string link);
    }
}
