using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    /// <summary>
    /// Interface that is used to get links to other documentation items.
    /// </summary>
    internal interface ILinkProvider
    {
        /// <summary>
        /// Tries to get a link to the output page for the specified item
        /// </summary>
        /// <param name="id">The id of the member to get a link to.</param>
        /// <param name="link">When a operation succeeds, contains the resulting link. Otherwise will be <c>null</c> after the operation.</param>
        /// <returns>Returns <c>true</c> is a link for the specified member could be created.</returns>
        bool TryGetLink(IDocument from, MemberId id, out Link? link);
    }
}
