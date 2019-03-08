namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    sealed class NullXmlDocsProvider : IXmlDocsProvider
    {
        public static readonly NullXmlDocsProvider Instance = new NullXmlDocsProvider();

        private NullXmlDocsProvider()
        { }

        public MemberElement TryGetDocumentationComments(MemberId id) => null;
    }
}
