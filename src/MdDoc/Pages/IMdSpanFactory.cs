using Grynwald.MarkdownGenerator;
using MdDoc.Model;

namespace MdDoc.Pages
{
    interface IMdSpanFactory
    {
        MdSpan CreateLink(MemberId target, MdSpan text);

        MdSpan GetMdSpan(MemberId id);

        MdSpan GetMdSpan(MemberId id, bool noLink);
    }
}
