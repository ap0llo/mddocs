using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<paramref>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// The <c>paramref</c> tag allows referencing a method's parameter.
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class ParamRefElement : Element, IEquatable<ParamRefElement>
    {
        /// <summary>
        /// Gets the name of the parameter being referenced.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ParamRefElement"/>.
        /// </summary>
        /// <param name="name">The name of the parameter being referenced.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c> or empty.</exception>
        public ParamRefElement(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value cannot be null or empty", nameof(name));

            Name = name;
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);

        /// <inheritdoc />
        public bool Equals(ParamRefElement? other) => other != null && StringComparer.Ordinal.Equals(Name, other.Name);

        /// <inheritdoc />
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as ParamRefElement);
    }
}
