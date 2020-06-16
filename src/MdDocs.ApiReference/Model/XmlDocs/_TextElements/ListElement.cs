using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<list>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public sealed class ListElement : Element, IEquatable<ListElement>
    {
        /// <summary>
        /// Gets the type of the list.
        /// </summary>
        public ListType Type { get; }

        /// <summary>
        /// Gets the list header.
        /// </summary>
        /// <value>
        /// The list header of <c>null</c> is no list header was specified.
        /// </value>
        public ListItemElement? ListHeader { get; }

        /// <summary>
        /// Gets the list's items.
        /// </summary>
        public IReadOnlyList<ListItemElement> Items { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ListElement"/>
        /// </summary>
        /// <param name="type">The list's type.</param>
        /// <param name="listHeader">The list's header. Can be <c>null</c></param>
        /// <param name="items">The list's element.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public ListElement(ListType type, ListItemElement? listHeader, IReadOnlyList<ListItemElement> items)
        {
            Type = type;
            ListHeader = listHeader;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public bool Equals(ListElement? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Type != other.Type)
                return false;

            if (ListHeader != null)
            {
                if (!ListHeader.Equals(other.ListHeader))
                    return false;
            }
            else
            {
                if (other.ListHeader != null)
                    return false;
            }

            return Items.SequenceEqual(other.Items);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Type.GetHashCode() * 397;
                if (ListHeader != null)
                    hash ^= ListHeader.GetHashCode();

                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as ListElement);
    }
}
