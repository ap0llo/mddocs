using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<code>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public sealed class CodeElement : Element, IEquatable<CodeElement>
    {
        /// <summary>
        /// Gets the content of the code element.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets the language of the code sample.
        /// </summary>
        /// <value>
        /// The language of the code sample if it was specified or <c>null</c> is no language was specified.
        /// </value>
        public string? Language { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="CodeElement"/>
        /// </summary>
        /// <param name="content">The content of the element.</param>
        /// <param name="language">The language of the code sample. Can be <c>null</c></param>
        public CodeElement(string content, string? language)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Language = language;
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public bool Equals(CodeElement? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return StringComparer.Ordinal.Equals(Content, other.Content) &&
                   StringComparer.Ordinal.Equals(Language, other.Language);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = StringComparer.Ordinal.GetHashCode(Content) * 397;
                hash ^= Language == null ? 0 : StringComparer.Ordinal.GetHashCode(Language);
                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as CodeElement);
    }
}
