using Mono.Cecil;
using System.Linq;

namespace MdDoc.Model
{
    public static class TypeReferenceExtensions
    {
        public static TypeName ToTypeName(this TypeReference typeReference) => new TypeName(typeReference);

        public static MemberId ToMemberId(this TypeReference typeReference) =>
            typeReference.ToTypeId();

        public static TypeId ToTypeId(this TypeReference typeReference)
        {
            if(typeReference.HasGenericParameters)
            {
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));
                return new GenericTypeId(typeReference.Namespace, name, typeReference.GenericParameters.Count);
            }
            else if(typeReference is GenericInstanceType genericInstanceType)
            {
                var name = genericInstanceType.Name.Substring(0, genericInstanceType.Name.LastIndexOf('`'));

                var typeArguments = genericInstanceType.GenericArguments
                    .Select(x => x.ToTypeId())
                    .ToArray();

                return new GenericTypeInstanceId(
                    typeReference.Namespace, 
                    name, 
                    typeArguments);
            }
            else if (typeReference is ArrayType arrayType)
            {
                return new ArrayTypeId(arrayType.ElementType.ToTypeId(), arrayType.Dimensions.Count);
            }
            else if (typeReference is GenericParameter genericParameter)
            {
                if(genericParameter.DeclaringMethod != null)
                {
                    var index = genericParameter.DeclaringMethod.GenericParameters.IndexOf(genericParameter);
                    return new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, index);                
                }
                else
                {
                    var index = genericParameter.DeclaringType.GenericParameters.IndexOf(genericParameter);
                    return new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, index);
                }
            }
            else
            {
                return new SimpleTypeId(typeReference.Namespace, typeReference.Name);
            }
        }
    }
}
