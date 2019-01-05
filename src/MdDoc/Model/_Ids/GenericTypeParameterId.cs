using System;

namespace MdDoc.Model
{
    public sealed class GenericTypeParameterId : TypeId, IEquatable<GenericTypeParameterId>
    {
        public enum MemberKind
        {
            Type,
            Method
        }

        public MemberKind DefiningMemberKind { get; }

        public int Index { get; }

        public override string DisplayName => Name;

        public override bool IsVoid => false;


        public GenericTypeParameterId(MemberKind definingMemberKind, int index) : this(definingMemberKind, index, $"T{index + 1}")
        { }

        public GenericTypeParameterId(MemberKind definingMemberKind, int index, string displayName) : base("", displayName)
        {
            DefiningMemberKind = definingMemberKind;
            Index = index;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningMemberKind.GetHashCode() * 397;
                hash ^= Index.GetHashCode(); 
                return hash;
            }
        }

        public bool Equals(GenericTypeParameterId other) => 
            other != null && DefiningMemberKind == other.DefiningMemberKind && Index == other.Index;

        public override bool Equals(TypeId other) => Equals(other as GenericTypeParameterId);

        public override bool Equals(object obj) => Equals(obj as GenericTypeParameterId);        
    }
}
