using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    internal interface IMdSpanFactory
    {
        /// <summary>
        /// Attempts to create a link to the documentation page for the specified item with the specified type.
        /// </summary>
        /// <param name="target">The item to create a link to.</param>
        /// <param name="text">The link text.</param>
        /// <returns>Returns a link to the specified item or the unchanged text if no link to the documentation item could be created.</returns>
        MdSpan CreateLink(MemberId target, MdSpan text);

        /// <summary>
        /// Converts the specified id into a human-readable text span.
        /// If the output page for the specified item can be found, a link span is returned. 
        /// </summary>
        /// <param name="id">The id to convert into a span.</param>
        MdSpan GetMdSpan(MemberId id);

        /// <summary>
        /// Converts the specified id into a human-readable text span.
        /// If the output page for the specified item can be found, a link span is returned. 
        /// </summary>
        /// <param name="id">The id to convert into a span.</param>
        /// <param name="noLink">When set to <c>true</c> suppresses generation of links.</param>
        MdSpan GetMdSpan(MemberId id, bool noLink);
    }
}
