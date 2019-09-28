using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<para>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// The <c>para</c> tag an be used to structure a text block into paragraphs.
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class ParaElement : Element, IEquatable<ParaElement>
    {
        /// <summary>
        /// Gets the paragraphs's content.
        /// </summary>
        public TextBlock Text { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ParaElement"/>
        /// </summary>
        /// <param name="content">The paragraph's content.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
        public ParaElement(TextBlock text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode() => Text.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ParaElement);

        /// <inheritdoc />
        public bool Equals(ParaElement other) => other != null && Text.Equals(other.Text);
    }
}
