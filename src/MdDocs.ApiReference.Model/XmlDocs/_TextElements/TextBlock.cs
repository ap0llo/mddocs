#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a sequence of text elements read from XML documentation comments.
    /// </summary>
    public sealed class TextBlock : Element, IEquatable<TextBlock>
    {
        public static readonly TextBlock Empty = new TextBlock(Array.Empty<Element>());

        /// <summary>
        /// Gets whether the text block contains any elements
        /// </summary>
        public bool IsEmpty => Elements.Count == 0;

        /// <summary>
        /// Gets the text block's text elements.
        /// </summary>
        public IReadOnlyList<Element> Elements { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="elements">The text block's elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="elements"/> is <c>null</c>.</exception>
        public TextBlock(IEnumerable<Element> elements)
        {
            Elements = elements?.ToArray() ?? throw new ArgumentNullException(nameof(elements));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (Elements.Count == 0)
                return 0;

            unchecked
            {
                var hash = Elements[0].GetHashCode() * 397;
                for (var i = 1; i < Elements.Count; i++)
                {
                    hash ^= Elements[i].GetHashCode();
                }
                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as TextBlock);

        /// <inheritdoc />
        public bool Equals(TextBlock other) => other != null && Elements.SequenceEqual(other.Elements);
    }
}
