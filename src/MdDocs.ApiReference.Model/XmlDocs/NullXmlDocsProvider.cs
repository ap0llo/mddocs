#nullable disable

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Empty implementation of <see cref="IXmlDocsProvider"/>.
    /// </summary>
    /// <remarks>
    /// This implementation of <see cref="IXmlDocsProvider"/> will return <c>null</c>
    /// for all calls to <see cref="TryGetDocumentationComments(MemberId)"/>.
    /// <para>
    /// This class is a singleton.
    /// </para>
    /// </remarks>
    internal sealed class NullXmlDocsProvider : IXmlDocsProvider
    {
        /// <summary>
        /// Gets the singleton instance of <see cref="NullXmlDocsProvider"/>.
        /// </summary>
        public static readonly NullXmlDocsProvider Instance = new NullXmlDocsProvider();


        private NullXmlDocsProvider()
        { }


        /// <inheritdoc />
        public MemberElement TryGetDocumentationComments(MemberId id) => null;
    }
}
