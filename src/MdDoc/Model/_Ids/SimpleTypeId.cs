using System;
using System.Collections.Generic;

namespace MdDoc.Model
{
    /// <summary>
    /// Identifies a non-generic type
    /// </summary>
    public sealed class SimpleTypeId : TypeId, IEquatable<SimpleTypeId>
    {
        private static readonly IReadOnlyDictionary<string, string> s_BuiltInTypes = new Dictionary<string, string>()
        {
            { "System.Boolean", "bool" },
            { "System.Byte", "byte" },
            { "System.SByte", "sbyte" },
            { "System.Char", "char" },
            { "System.Decimal", "decimal" },
            { "System.Double", "double" },
            { "System.Single", "float" },
            { "System.Int32", "int" },
            { "System.UInt32", "uint" },
            { "System.Int64", "long" },
            { "System.UInt64", "ulong" },
            { "System.Object", "object" },
            { "System.Int16", "short" },
            { "System.UInt16", "ushort" },
            { "System.String", "string" },
            { "System.Void", "void" }
        };


        public override string DisplayName =>
            s_BuiltInTypes.TryGetValue(NamespaceAndName, out var builtinName)
                ? builtinName
                : Name;


        public SimpleTypeId(string namespaceName, string name) : base(namespaceName, name)
        { }


        public override bool Equals(object obj) => Equals(obj as SimpleTypeId);     

        public bool Equals(SimpleTypeId other) => Equals((TypeId)other);

        public override int GetHashCode() => base.GetHashCode();
    }
}
