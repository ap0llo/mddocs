namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    interface IXmlDocsProvider
    {
        MemberElement TryGetDocumentationComments(MemberId id);
    }
}
