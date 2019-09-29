using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c>seealso</c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// <c>seealso</c> allows specifying links that will show up in the "See Also" section.
    /// <para>
    /// Supported for types, fields, event, properties and methods
    /// </para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public class SeeAlsoElement
    {
        /// <summary>
        /// Gets the id of the member being referenced.
        /// </summary>
        public MemberId MemberId { get; }

        /// <summary>
        /// Gets the content of the <c>seealso</c> element.
        /// </summary>
        /// <value>
        /// The content of the <c>see</c> element or an empty text block if no text was specified.
        /// </value>
        public TextBlock Text { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/>.
        /// </summary>
        /// <param name="memberId">The if of the member being referenced.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        public SeeAlsoElement(MemberId memberId) : this(memberId, TextBlock.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/>.
        /// </summary>
        /// <param name="memberId">The if of the member being referenced.</param>
        /// <param name="text">The content of the <c>seealso</c> element. Can be <c>null</c></param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
        public SeeAlsoElement(MemberId memberId, TextBlock text)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
