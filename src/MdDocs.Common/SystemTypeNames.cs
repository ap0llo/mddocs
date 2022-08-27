#pragma warning disable IDE1006 // Naming Styles: Ignore hint that const strings should be prefixed with 's_' as all these fields are public
namespace Grynwald.MdDocs.Common
{
    /// <summary>
    /// Defines shared constants for names of .NET System types and attributes emitted by the compiler
    /// </summary>
    public static class SystemTypeNames
    {
        // Built-in types / types with special meaning in C#
        public const string StringFullName = "System.String";
        public const string CharFullName = "System.Char";
        public const string ValueTypeFullName = "System.ValueType";
        public const string ObjectFullName = "System.Object";
        public const string EnumFullName = "System.Enum";
        public const string Int32FullName = "System.Int32";
        public const string BooleanFullName = "System.Boolean";
        public const string SingleFullName = "System.Single";
        public const string DoubleFullName = "System.Double";
        public const string DecimalFullName = "System.Decimal";
        public const string ByteFullName = "System.Byte";
        public const string NullableFullName = "System.Nullable`1";

        // Common Attribute types
        public const string ExtensionAttributeFullName = "System.Runtime.CompilerServices.ExtensionAttribute";
        public const string FlagsAttributeFullName = "System.FlagsAttribute";
        public const string DefaultMemberAttributeFullName = "System.Reflection.DefaultMemberAttribute";
        public const string IsReadOnlyAttributeFullName = "System.Runtime.CompilerServices.IsReadOnlyAttribute";
        public const string ObsoleteAttributeFullName = "System.ObsoleteAttribute";
        public const string ParamArrayAttributeFullName = "System.ParamArrayAttribute";
        public const string AssemblyTitleAttributeFullName = "System.Reflection.AssemblyTitleAttribute";
        public const string AssemblyInformationalVersionAttribute = "System.Reflection.AssemblyInformationalVersionAttribute";
        public const string DecimalConstantAttribute = "System.Runtime.CompilerServices.DecimalConstantAttribute";
    }
}
#pragma warning restore IDE1006 // Naming Styles
