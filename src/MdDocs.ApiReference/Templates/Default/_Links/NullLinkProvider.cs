using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    /// <summary>
    /// Default implementation of <see cref="ILinkProvider"/>.
    /// Returns <c>false</c> for all <see cref="ILinkProvider.TryGetLink"/> calls.
    /// </summary>
    internal class NullLinkProvider : ILinkProvider
    {
        /// <summary>
        /// The singleton instance of <see cref="NullLinkProvider"/>
        /// </summary>
        public static readonly NullLinkProvider Instance = new NullLinkProvider();


        private NullLinkProvider()
        { }


        public bool TryGetLink(IDocument from, MemberId id, out Link? link)
        {
            link = default;
            return false;
        }
    }
}
