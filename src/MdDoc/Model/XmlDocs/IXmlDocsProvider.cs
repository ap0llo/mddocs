using Mono.Cecil;

namespace MdDoc.Model.XmlDocs
{
    interface IXmlDocsProvider
    {
        MemberElement TryGetDocumentationComments(TypeReference type);

        MemberElement TryGetDocumentationComments(MethodDefinition method);

        MemberElement TryGetDocumentationComments(PropertyDefinition property);

        MemberElement TryGetDocumentationComments(FieldDefinition field);

        MemberElement TryGetDocumentationComments(EventDefinition ev);
    }
}
