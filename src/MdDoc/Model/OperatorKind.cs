using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
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
        ImplicitConversion,
        ExplicitConversion
    }
}
