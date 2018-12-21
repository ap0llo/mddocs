using Mono.Cecil;
using System;
using System.Linq;

namespace MdDoc.Model
{
    public static class MethodReferenceExtensions
    {
        public static bool IsOperatorOverload(this MethodReference methodReference) =>
            methodReference.GetOperatorKind().HasValue;

        public static OperatorKind? GetOperatorKind(this MethodReference methodReference) =>
            OperatorMethodNames.GetOperatorKind(methodReference.Name);
               
        public static MemberId ToMemberId(this MethodReference method)
        {
            var parameters = method.Parameters.Count > 0
                ? method.Parameters.Select(p => p.ParameterType.ToTypeId()).ToArray()
                : Array.Empty<TypeId>();

            TypeId returnType = default;
            var operatorKind = method.GetOperatorKind();
            if(operatorKind == OperatorKind.ImplicitConversion || operatorKind == OperatorKind.ExplicitConversion)
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
