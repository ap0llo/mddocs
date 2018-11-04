using System.Xml.Linq;
using Grynwald.MarkdownGenerator;
using Mono.Cecil;

namespace MdDoc
{
    interface IXmlDocProvider
    {
        MemberDocumentation TryGetDocumentation(MemberReference member);
    }
}