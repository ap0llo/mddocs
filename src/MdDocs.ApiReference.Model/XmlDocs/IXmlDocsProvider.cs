namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Interface for components that provide data from XML documentation comments.
    /// </summary>
    internal interface IXmlDocsProvider
    {
        /// <summary>
        /// Gets the documentation for the specified member.
        /// </summary>
        /// <param name="id">The id of the member to retrieve documentation for.</param>
        /// <returns>Returns the documentation for the specified member of <c>null</c> if no documentation was found.</returns>
        MemberElement? TryGetDocumentationComments(MemberId id);
    }
}
