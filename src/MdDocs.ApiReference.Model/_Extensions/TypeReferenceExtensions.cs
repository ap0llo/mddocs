﻿using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="TypeReference"/>.
    /// </summary>
    internal static class TypeReferenceExtensions
    {
        /// <summary>
        /// Gets the <see cref="MemberId"/> for the specified type.
        /// </summary>
        public static MemberId ToMemberId(this TypeReference typeReference) => typeReference.ToTypeId();

        /// <summary>
        /// Gets the <see cref="TypeId"/> for the specified type.
        /// </summary>
        public static TypeId ToTypeId(this TypeReference typeReference)
        {
            // generic instance type: a generic type with type arguments
            // does not necessarily mean the type arguments are real types
            // they can still be type parameters
            if (typeReference is GenericInstanceType genericInstanceType)
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                var typeArguments = genericInstanceType.GenericArguments
                    .Select(x => x.ToTypeId())
                    .ToArray();

                return new GenericTypeInstanceId(
                    new NamespaceId(typeReference.Namespace),
                    name,
                    typeArguments);
                
            }
            // Unbound generic type, e.g. 
            // class Foo<T> 
            // { }        
            else if (typeReference.HasGenericParameters)
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                // get the names of the type parameters
                // so the GenericTypeId's DisplayName matches
                // the definition
                var typeParameterNames = typeReference.GenericParameters.Select(x => x.Name).ToArray();

                return new GenericTypeId(new NamespaceId(typeReference.Namespace), name, typeReference.GenericParameters.Count, typeParameterNames);
            }
            // type is an generic parameter, e.g. the "T" in 
            // class Foo<T> 
            // { }  
            else if (typeReference is GenericParameter genericParameter)
            {
                // parameter is declared at the method level
                if (genericParameter.DeclaringMethod != null)
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
                return new SimpleTypeId(new NamespaceId(typeReference.Namespace), typeReference.Name);
            }
        }
    }
}