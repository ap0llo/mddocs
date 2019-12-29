using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<item>]]></c> or <c><![CDATA[<listheader>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public sealed class ListItemElement : Element, IEquatable<ListItemElement>
    {
        /// <summary>
        /// The term described by the list item.
        /// </summary>
        /// <value>
        /// The content of the <c>term></c> element if it was specified or a empty text blco
        /// </value>
        public TextBlock Term { get; }

        /// <summary>
        /// Gets the list item's content.
        /// </summary>
        public TextBlock Description { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ListItemElement"/>
        /// </summary>
        /// <param name="term">The content of the list items <c>term</c> element. Can be <c>null</c></param>
        /// <param name="description">The content of the list items <c>description</c> element.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="description"/> is <c>null</c>.</exception>
        public ListItemElement(TextBlock? term, TextBlock description)
        {
            Term = term ?? TextBlock.Empty;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Description.GetHashCode() * 397;
                hash ^= Term.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ListItemElement);

        /// <inheritdoc />
        public bool Equals(ListItemElement? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Term.Equals(other.Term) && Description.Equals(other.Description);
        }
    }
}
