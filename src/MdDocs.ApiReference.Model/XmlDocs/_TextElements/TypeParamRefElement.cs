using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<typeparamref>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// The <c>para</c> tag can be used to reference a method's or type's generic type parameter.
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class TypeParamRefElement : Element, IEquatable<TypeParamRefElement>
    {
        /// <summary>
        /// Gets the name of the type parameter being referenced.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="TypeParamRefElement"/>.
        /// </summary>
        /// <param name="name">The name of the type parameter being referenced.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
        public TypeParamRefElement(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as TypeParamRefElement);

        /// <inheritdoc />
        public bool Equals(TypeParamRefElement other) => other != null && StringComparer.Ordinal.Equals(Name, other.Name);
    }
}
