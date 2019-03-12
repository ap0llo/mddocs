using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a sequence of text elements read from XML documentation comments.
    /// </summary>
    public sealed class TextBlock : Element
    {
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
    }
}
