using MdDoc.Model;

namespace MdDoc.Pages
{
    interface ILinkProvider
    {
        bool TryGetLink(MemberId id, out string link);
    }
}
