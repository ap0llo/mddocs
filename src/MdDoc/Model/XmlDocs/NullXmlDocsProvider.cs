namespace MdDoc.Model.XmlDocs
{
    class NullXmlDocsProvider : IXmlDocsProvider
    {
        public MemberElement TryGetDocumentationComments(MemberId id) => null;
    }
}
