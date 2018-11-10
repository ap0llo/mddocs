using System.Xml.Linq;
using Grynwald.MarkdownGenerator;
using Mono.Cecil;

namespace MdDoc
{
    public interface IXmlDocProvider
    {
        MemberDocumentation TryGetDocumentation(MemberReference member);
    }
}