using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    /// <summary>
    /// Identifies an unbound generic type
    /// </summary>
    public sealed class GenericTypeId : TypeId, IEquatable<GenericTypeId>
    {
        private readonly IReadOnlyList<string> m_TypeParameterDisplayNames;


        public int Arity { get; }

        public override string DisplayName => $"{Name}<{String.Join(", ", m_TypeParameterDisplayNames)}>";

        public override bool IsVoid => false;

        public GenericTypeId(string namespaceName, string name, int arity, IReadOnlyList<string> typeParameterDisplayNames)
            : this(new NamespaceId(namespaceName), name, arity, typeParameterDisplayNames)
        { }

        public GenericTypeId(NamespaceId @namespace, string name, int arity, IReadOnlyList<string> typeParameterDisplayNames) : base(@namespace, name)
        {
            m_TypeParameterDisplayNames = typeParameterDisplayNames ?? throw new ArgumentNullException(nameof(typeParameterDisplayNames));
            Arity = arity;

            if (typeParameterDisplayNames?.Count != arity)
                throw new ArgumentException("The number of type parameter display names must match the type's arity", nameof(typeParameterDisplayNames));
        }

        public GenericTypeId(string namespaceName, string name, int arity) : this(new NamespaceId(namespaceName), name, arity)
        { }

        public GenericTypeId(NamespaceId @namespace, string name, int arity) : base(@namespace, name)
        {
            Arity = arity;
            m_TypeParameterDisplayNames = Enumerable.Range(1, arity).Select(x => "T" + x).ToArray();
        }


        public override bool Equals(object obj) => Equals(obj as GenericTypeId);

        public override bool Equals(TypeId other) => Equals(other as GenericTypeId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash ^= Arity.GetHashCode();                
                return hash;
            }
        }

        public bool Equals(GenericTypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return base.Equals(other) && Arity == other.Arity;
        }
    }
}
