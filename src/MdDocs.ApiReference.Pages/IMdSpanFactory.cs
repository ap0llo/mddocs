using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    interface IMdSpanFactory
    {
        MdSpan CreateLink(MemberId target, MdSpan text);

        MdSpan GetMdSpan(MemberId id);

        MdSpan GetMdSpan(MemberId id, bool noLink);
    }
}
