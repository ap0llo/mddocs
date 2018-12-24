using System;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public static class MethodReferenceExtensions
    {
        public static bool IsOperator(this MethodReference methodReference) =>
            methodReference.GetOperatorKind().HasValue;

        public static bool IsOperator(this MethodReference methodReference, out OperatorKind operatorKind)
        {
            var kind = methodReference.GetOperatorKind();
            if(kind.HasValue)
            {
                operatorKind = kind.Value;
                return true;
            }
            else
            {
                operatorKind = (OperatorKind) (-1);
                return false;
            }
        }
        
        public static OperatorKind? GetOperatorKind(this MethodReference methodReference) =>
            OperatorMethodNames.GetOperatorKind(methodReference.Name);
               
        public static MemberId ToMemberId(this MethodReference method)
        {
            var parameters = method.Parameters.Count > 0
                ? method.Parameters.Select(p => p.ParameterType.ToTypeId()).ToArray()
                : Array.Empty<TypeId>();

            TypeId returnType = default;
            var operatorKind = method.GetOperatorKind();
            if(operatorKind == OperatorKind.Implicit || operatorKind == OperatorKind.Explicit)
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
