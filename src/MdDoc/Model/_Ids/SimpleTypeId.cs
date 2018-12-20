using System;

namespace MdDoc.Model
{
    /// <summary>
    /// Identifies a non-generic type
    /// </summary>
    public class SimpleTypeId : TypeId, IEquatable<SimpleTypeId>
    {
        public SimpleTypeId(string namespaceName, string name) : base(namespaceName, name)
        { }


        public override bool Equals(object obj) => Equals(obj as SimpleTypeId);     

        public bool Equals(SimpleTypeId other) => Equals((TypeId)other);

        public override int GetHashCode() => base.GetHashCode();
    }
}
