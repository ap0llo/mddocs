using System;

namespace MdDoc.Model
{
    public sealed class NamespaceId : MemberId, IEquatable<NamespaceId>
    {
        public string Name { get; }


        public bool IsSystem => Name == "System";


        public NamespaceId(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        public bool Equals(NamespaceId other) => other != null && StringComparer.Ordinal.Equals(Name, other.Name);

        public override bool Equals(object obj) => Equals(obj as NamespaceId);

        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);
    }
}
