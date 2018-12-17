using Mono.Cecil;

namespace MdDoc.Model.XmlDocs
{
    interface IXmlDocsProvider
    {
        MemberElement TryGetDocumentationComments(TypeReference type);

        MemberElement TryGetDocumentationComments(MethodDefinition method);

        MemberElement TryGetDocumentationComments(PropertyReference property);

        MemberElement TryGetDocumentationComments(FieldReference field);

        MemberElement TryGetDocumentationComments(EventReference ev);
    }
}
