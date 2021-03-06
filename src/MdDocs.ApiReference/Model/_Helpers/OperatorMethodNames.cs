﻿namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Helper class to get the <see cref="OperatorKind"/> from a method name.
    /// </summary>
    internal static class OperatorMethodNames
    {
        /// <summary>
        /// Gets the operator being overloaded by a method with the specified name.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>Returns the operator being overloaded or <c>null</c> if the method does not overload an operator.</returns>
        public static OperatorKind? GetOperatorKind(string methodName)
        {
            switch (methodName)
            {
                case "op_UnaryPlus":
                    return OperatorKind.UnaryPlus;
                case "op_UnaryNegation":
                    return OperatorKind.UnaryNegation;
                case "op_LogicalNot":
                    return OperatorKind.LogicalNot;
                case "op_OnesComplement":
                    return OperatorKind.OnesComplement;
                case "op_Increment":
                    return OperatorKind.Increment;
                case "op_Decrement":
                    return OperatorKind.Decrement;
                case "op_True":
                    return OperatorKind.True;
                case "op_False":
                    return OperatorKind.False;
                case "op_Addition":
                    return OperatorKind.Addition;
                case "op_Subtraction":
                    return OperatorKind.Subtraction;
                case "op_Multiply":
                    return OperatorKind.Multiply;
                case "op_Division":
                    return OperatorKind.Division;
                case "op_Modulus":
                    return OperatorKind.Modulus;
                case "op_BitwiseAnd":
                    return OperatorKind.BitwiseAnd;
                case "op_BitwiseOr":
                    return OperatorKind.BitwiseOr;
                case "op_ExclusiveOr":
                    return OperatorKind.ExclusiveOr;
                case "op_LeftShift":
                    return OperatorKind.LeftShift;
                case "op_RightShift":
                    return OperatorKind.RightShift;
                case "op_Equality":
                    return OperatorKind.Equality;
                case "op_Inequality":
                    return OperatorKind.Inequality;
                case "op_LessThan":
                    return OperatorKind.LessThan;
                case "op_GreaterThan":
                    return OperatorKind.GreaterThan;
                case "op_LessThanOrEqual":
                    return OperatorKind.LessThanOrEqual;
                case "op_GreaterThanOrEqual":
                    return OperatorKind.GreaterThanOrEqual;
                case "op_Implicit":
                    return OperatorKind.Implicit;
                case "op_Explicit":
                    return OperatorKind.Explicit;

                default:
                    return null;
            }
        }
    }
}
