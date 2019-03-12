using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a class' or method's type parameter
    /// </summary>
    public sealed class GenericTypeParameterId : TypeId, IEquatable<GenericTypeParameterId>
    {
        public enum MemberKind
        {
            Type,
            Method
        }

        /// <summary>
        /// Gets the kind of member (either type or method) that defines the type parameter.
        /// </summary>
        public MemberKind DefiningMemberKind { get; }

        /// <summary>
        /// Gets the index of the type parameter within the defining members type parameters.
        /// </summary>
        public int Index { get; }

        /// <inheritdoc />
        public override string DisplayName => Name;

        /// <inheritdoc />
        public override bool IsVoid => false;


        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeParameterId"/>.
        /// </summary>
        /// <param name="definingMemberKind">The kind of member (either type or method) that defined the parameter.</param>
        public GenericTypeParameterId(MemberKind definingMemberKind, int index) : this(definingMemberKind, index, $"T{index + 1}")
        { }

        /// <param name="index">The index of this parameter within the list of type parameters defined by the defining member.</param>/// <summary>
        /// Initializes a new instance of <see cref="GenericTypeParameterId"/>.
        /// </summary>
        /// <param name="definingMemberKind">The kind of member (either type or method) that defined the parameter.</param>
        /// <param name="index">The index of this parameter within the list of type parameters defined by the defining member.</param>
        /// <param name="displayName">The display name of the type parameter (e.g. <c>TValue</c>, <c>TKey</c> ...).</param>
        public GenericTypeParameterId(MemberKind definingMemberKind, int index, string displayName) : base(new NamespaceId(""), displayName)
        {
            DefiningMemberKind = definingMemberKind;
            Index = index;
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningMemberKind.GetHashCode() * 397;
                hash ^= Index.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public override bool Equals(TypeId other) => Equals(other as GenericTypeParameterId);

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as GenericTypeParameterId);

        /// <summary>
        /// Determines whether this instance of <see cref="GenericTypeParameterId"/> refers to the same type parameter as <paramref name="other"/>.
        /// </summary>
        public bool Equals(GenericTypeParameterId other)
        {
            return other != null &&
                   DefiningMemberKind == other.DefiningMemberKind &&
                   Index == other.Index;
        }
    }
}
