using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    /// <summary>
    /// Identifies a constructed type (a generic type with type arguments)
    /// </summary>
    public sealed class GenericTypeInstanceId : TypeId, IEquatable<GenericTypeInstanceId>
    {
        public IReadOnlyList<TypeId> TypeArguments { get; }
        
        public override string DisplayName =>
            $"{Name}<{String.Join(", ", TypeArguments.Select(a => a.DisplayName))}>";

        public override bool IsVoid => false;


        public GenericTypeInstanceId(string namespaceName, string name, IReadOnlyList<TypeId> typeArguments)
            : this(new NamespaceId(namespaceName), name, typeArguments)
        { }

        public GenericTypeInstanceId(NamespaceId @namespace, string name, IReadOnlyList<TypeId> typeArguments) : base(@namespace, name)
        {
            TypeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }


        public override bool Equals(TypeId other) => Equals(other as GenericTypeInstanceId);

        public override bool Equals(object obj) => Equals(obj as GenericTypeInstanceId);

        public bool Equals(GenericTypeInstanceId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return base.Equals(other) && TypeArguments.SequenceEqual(other.TypeArguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();

                foreach (var argument in TypeArguments)
                {
                    hash ^= argument.GetHashCode();
                }

                return hash;
            }
        }
    }
}
