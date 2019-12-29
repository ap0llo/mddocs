#nullable disable

using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a namespace.
    /// </summary>
    public sealed class NamespaceId : MemberId, IEquatable<NamespaceId>
    {
        public static readonly NamespaceId GlobalNamespace = new NamespaceId("");

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Determines whether this instance of <see cref="NamespaceId"/> is the <c>System</c> namespace.
        /// </summary>
        public bool IsSystem => Name == "System";


        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceId"/>.
        /// </summary>
        /// <param name="name">The name of the namespace.</param>
        public NamespaceId(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as NamespaceId);

        /// <inheritdoc />
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

        /// <summary>
        /// Determines whether this instance of <see cref="NamespaceId"/> refers to the same namespace as <paramref name="other"/>.
        /// </summary>
        public bool Equals(NamespaceId other)
        {
            return other != null &&
                   StringComparer.Ordinal.Equals(Name, other.Name);
        }
    }
}
