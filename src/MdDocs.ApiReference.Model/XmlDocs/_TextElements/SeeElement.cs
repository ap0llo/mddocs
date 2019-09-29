using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<see>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// The <c>see</c> tag can be used to reference other types or methods.
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class SeeElement : Element, IEquatable<SeeElement>
    {
        /// <summary>
        /// Gets the id of the member being referenced.
        /// </summary>
        public MemberId MemberId { get; }

        /// <summary>
        /// Gets the text of the <c>see</c> element.
        /// </summary>
        /// <value>
        /// The content of the <c>see</c> element or an empty text block if no text was specified.
        /// </value>
        public TextBlock Text { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="SeeElement"/>.
        /// </summary>
        /// <param name="memberId">The id of the member being referenced.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        public SeeElement(MemberId memberId) : this(memberId, TextBlock.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SeeElement"/>.
        /// </summary>
        /// <param name="memberId">The id of the member being referenced.</param>
        /// <param name="text">The element's content.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        public SeeElement(MemberId memberId, TextBlock text)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = MemberId.GetHashCode() * 397;
                hash ^= Text != null ? Text.GetHashCode() : 0;                
                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as SeeElement);

        /// <inheritdoc />
        public bool Equals(SeeElement other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!MemberId.Equals(other.MemberId))
                return false;

            if(Text == null)
            {
                return other.Text == null;
            }
            else
            {
                return Text.Equals(other.Text);
            }
        }
    }
}
