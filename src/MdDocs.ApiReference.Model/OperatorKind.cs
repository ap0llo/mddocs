namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Defines the operators that can be overloaded in C#
    /// </summary>
    /// <remarks>
    /// For documentation on operator overloading in C#, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/overloadable-operators
    /// </remarks>
    public enum OperatorKind
    {
        UnaryPlus,
        UnaryNegation,
        LogicalNot,
        OnesComplement,
        Increment,
        Decrement,
        True,
        False,
        Addition,
        Subtraction,
        Multiply,
        Division,
        Modulus,
        BitwiseAnd,
        BitwiseOr,
        ExclusiveOr,
        LeftShift,
        RightShift,
        Equality,
        Inequality,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        Implicit,
        Explicit
    }
}
