// Defines additional attributes for C# 8's nullable reference types
// These types are built-in to .NET Core 3 and later, but not available on .NET Standard 2.0 or net461
// To avoid having to #ifdef every usage of the attributes, they are defined here as internal for .NET Core 2.1
#if NETSTANDARD2_0 || NET461

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute : Attribute
    {
        public bool ReturnValue { get; }


        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    public sealed class MaybeNullAttribute : Attribute
    {
    }
}

#endif
