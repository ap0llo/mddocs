using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public static class TypeReferenceExtensions
    {      
        public static MemberId ToMemberId(this TypeReference typeReference) => typeReference.ToTypeId();

        public static TypeId ToTypeId(this TypeReference typeReference)
        {             
            // generic instance type: a generic type with type arguments
            // does not necessarily mean the type arguemtns are real types
            // they can still be type parameters
            if(typeReference is GenericInstanceType genericInstanceType)
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                // type arguments are parameters
                // e.g. in
                //  class Foo<T>
                //  {
                //      public List<T> Bar() => null;
                //  }
                //
                //  The return type of Bar is a GenericInstanceType, but bound to 
                //  the classes parameter
                //
                if (typeReference.ContainsGenericParameter)
                {
                    
                    var typeArity = genericInstanceType.GenericArguments.Count;

                    // get the names of the type parameters
                    // so the GenericTypeId's DisplayName matches
                    // the definition
                    var typeParameterNames = genericInstanceType
                        .GenericArguments
                        .Select(x => x.Name).ToArray();

                    return new GenericTypeId(typeReference.Namespace, name, typeArity, typeParameterNames);
                }
                // Type arguments are bound to real types
                else
                {
                    var typeArguments = genericInstanceType.GenericArguments
                        .Select(x => x.ToTypeId())
                        .ToArray();

                    return new GenericTypeInstanceId(
                        typeReference.Namespace,
                        name,
                        typeArguments);
                }
            }
            // Unbound generic type, e.g. 
            // class Foo<T> 
            // { }        
            else if(typeReference.HasGenericParameters)
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                // get the names of the type parameters
                // so the GenericTypeId's DisplayName matches
                // the definition
                var typeParameterNames = typeReference.GenericParameters.Select(x => x.Name).ToArray();

                return new GenericTypeId(typeReference.Namespace, name, typeReference.GenericParameters.Count, typeParameterNames);
            }
            // type is an generic parameter, e.g. the "T" in 
            // class Foo<T> 
            // { }  
            else if (typeReference is GenericParameter genericParameter)
            {
                // parameter is declared at the method level
                if(genericParameter.DeclaringMethod != null)
                {
                    var index = genericParameter.DeclaringMethod.GenericParameters.IndexOf(genericParameter);
                    return new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Method, index, genericParameter.Name);                
                }
                // parameter is declared at the type-level
                else
                {
                    var index = genericParameter.DeclaringType.GenericParameters.IndexOf(genericParameter);
                    return new GenericTypeParameterId(GenericTypeParameterId.MemberKind.Type, index, genericParameter.Name);
                }
            }
            // type is an array type
            else if (typeReference is ArrayType arrayType)
            {
                return new ArrayTypeId(arrayType.ElementType.ToTypeId(), arrayType.Dimensions.Count);
            }
            // type is a "normal", non-generic type
            else
            {
                return new SimpleTypeId(typeReference.Namespace, typeReference.Name);
            }
        }
    }
}
