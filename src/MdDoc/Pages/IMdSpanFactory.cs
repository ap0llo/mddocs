using Grynwald.MarkdownGenerator;
using MdDoc.Model;

namespace MdDoc.Pages
{
    interface IMdSpanFactory
    {
        MdSpan GetMdSpan(MemberId type);

        MdSpan GetMdSpan(MemberId type, bool noLink);
    }
}
