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
        /// Initializes a new instance of <see cref="SeeElement"/>.
        /// </summary>
        /// <param name="memberId">The id of the member being referenced.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        public SeeElement(MemberId memberId)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode() => MemberId.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as SeeElement);

        /// <inheritdoc />
        public bool Equals(SeeElement other) => other != null && MemberId.Equals(other.MemberId);
    }
}
