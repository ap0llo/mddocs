using System;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="MethodReference"/>.
    /// </summary>
    internal static class MethodReferenceExtensions
    {
        /// <summary>
        /// Determines whether the specified method is an operator overload.
        /// </summary>
        /// <seealso cref="OperatorKind"/>
        public static bool IsOperator(this MethodReference methodReference) =>
            methodReference.GetOperatorKind().HasValue;

        /// <summary>
        /// Determines whether the specified method is an operator overload and saves the operator kind into the <paramref name="operatorKind"/> out parameter.
        /// </summary>
        /// <seealso cref="OperatorKind"/>
        public static bool IsOperator(this MethodReference methodReference, out OperatorKind operatorKind)
        {
            var kind = methodReference.GetOperatorKind();

            if (kind.HasValue)
            {
                operatorKind = kind.Value;
                return true;
            }
            else
            {
                operatorKind = (OperatorKind)(-1);
                return false;
            }
        }

        /// <summary>
        /// Gets the <see cref="OperatorKind"/> for a method if the method is an operator overload.
        /// </summary>
        /// <returns>Returns the kind of operator the method implements or <c>null</c> if the method is not an operator overload.</returns>
        /// <seealso cref="OperatorKind"/>
        public static OperatorKind? GetOperatorKind(this MethodReference methodReference) =>
            OperatorMethodNames.GetOperatorKind(methodReference.Name);

        /// <summary>
        /// Gets the <see cref="MemberId"/> for the specified method.
        /// </summary>
        public static MemberId ToMemberId(this MethodReference method) => method.ToMethodId();

        /// <summary>
        /// Gets the <see cref="MethodId"/> for the specified method.
        /// </summary>
        public static MethodId ToMethodId(this MethodReference method)
        {
            var parameters = method.Parameters.Count > 0
                ? method.Parameters.Select(p => p.ParameterType.ToTypeId()).ToArray()
                : Array.Empty<TypeId>();

            // return type is only included in the method id if it is a explicit or implicit conversion overload
            TypeId? returnType = default;
            var operatorKind = method.GetOperatorKind();
            if (operatorKind == OperatorKind.Implicit || operatorKind == OperatorKind.Explicit)
            {
                returnType = method.ReturnType.ToTypeId();
            }

            return new MethodId(
                method.DeclaringType.ToTypeId(),
                method.Name,
                method.GenericParameters.Count,
                parameters,
                returnType
            );
        }
    }
}
