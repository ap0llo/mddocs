namespace MdDoc.Model.XmlDocs
{
    interface IXmlDocsProvider
    {
        MemberElement TryGetDocumentationComments(MemberId id);
    }
}
