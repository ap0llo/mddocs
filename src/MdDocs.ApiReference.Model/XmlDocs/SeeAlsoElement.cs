using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c>seealso</c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// <c>seealso</c> allows specifying links that will show up in the "See Also" section.
    /// <para>
    /// <para>
    /// While Visual Studio only allows referring to other code elements using the <c>cref</c> attribute,
    /// linking to external resources (e.g. websites) is supported by as well using the <c>href</c> attribute.
    /// </para>
    /// <para>
    /// When a <c>cref</c> attribute is present, the <c>href</c> attribute is ignored.
    /// </para>
    /// Supported for types, fields, event, properties, indexers and methods.
    /// </para>
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class SeeAlsoElement : IEquatable<SeeAlsoElement>
    {
        /// <summary>
        /// Gets the id of the member being referenced (using the <c>cref</c> attribute).
        /// </summary>
        /// <value>The id of the member being referenced or <c>null</c>, if the element references an external resource.</value>
        public MemberId? MemberId { get; }

        /// <summary>
        /// Gets the target of the external resource being referenced (using the <c>href</c> attribute).
        /// </summary>
        /// <value>The uri of the external resource or <c>null</c>, if the element references a assembly member.</value>
        public Uri? Target { get; }

        /// <summary>
        /// Gets the content of the <c>seealso</c> element.
        /// </summary>
        /// <value>
        /// The content of the <c>see</c> element or an empty text block if no text was specified.
        /// </value>
        public TextBlock Text { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/> referencing a code element (using the <c>cref</c> attribute).
        /// </summary>
        /// <param name="memberId">The id of the member being referenced.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        public SeeAlsoElement(MemberId memberId) : this(memberId, TextBlock.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/> referencing a code element (using the <c>cref</c> attribute)
        /// </summary>
        /// <param name="memberId">The id of the member being referenced.</param>
        /// <param name="text">The content of the <c>seealso</c> element.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
        public SeeAlsoElement(MemberId memberId, TextBlock text)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }


        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/> referencing an external resource (using the <c>href</c> attribute).
        /// </summary>
        /// <param name="target">The uri of the external resource.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c>.</exception>
        public SeeAlsoElement(Uri target) : this(target, TextBlock.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SeeAlsoElement"/> referencing an external resource (using the <c>href</c> attribute).
        /// </summary>
        /// <param name="target">The uri of the external resource.</param>
        /// <param name="text">The content of the <c>seealso</c> element.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
        public SeeAlsoElement(Uri target, TextBlock text)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }


        public override int GetHashCode()
        {
            unchecked
            {
                if(MemberId != null)
                {
                    var hash = MemberId.GetHashCode() * 397;
                    hash ^= Target != null ? Target.GetHashCode() : 0;
                    hash ^= Text.GetHashCode();
                    return hash;
                }
                else
                {
                    var hash = Target!.GetHashCode() * 397;
                    hash ^= MemberId != null ? MemberId.GetHashCode() : 0;
                    hash ^= Text.GetHashCode();
                    return hash;
                }
            }
        }

        public override bool Equals(object obj) => Equals(obj as SeeAlsoElement);

        public bool Equals(SeeAlsoElement? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ((MemberId == null && other.MemberId == null) || (MemberId != null && MemberId.Equals(other.MemberId))) &&
                   ((Target == null && other.Target == null) || (Target != null && Target.Equals(other.Target))) &&
                   Text.Equals(other.Text);
        }
    }
}
