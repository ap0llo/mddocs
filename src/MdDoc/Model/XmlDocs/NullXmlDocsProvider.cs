namespace MdDoc.Model.XmlDocs
{
    class NullXmlDocsProvider : IXmlDocsProvider
    {
        public static readonly NullXmlDocsProvider Instance = new NullXmlDocsProvider();

        private NullXmlDocsProvider()
        { }

        public MemberElement TryGetDocumentationComments(MemberId id) => null;
    }
}
