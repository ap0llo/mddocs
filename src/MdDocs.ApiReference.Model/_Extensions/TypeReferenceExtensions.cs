using System.Collections.Generic;
using System.Linq;
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
        public static TypeId ToTypeId(this TypeReference typeReference) => typeReference.ToTypeId(Enumerable.Empty<TypeReference>());

        private static TypeId ToTypeId(this TypeReference typeReference, IEnumerable<TypeReference> genericTypeArguments)
        {
            // generic instance type: a generic type with type arguments
            // does not necessarily mean the type arguments are real types
            // they can still be type parameters            
            if (typeReference is GenericInstanceType genericInstanceType)
            {
                return GetGenericTypeInstanceId(genericInstanceType);
            }
            // handle a type with type parameters but explicitly specified type arguments
            // this can happen when converting generic nested types
            // e.g.
            // for the type OuterClass<string>.NestedClass<int>, the structure looks like this
            //   NestedClass: 
            //     Type Arguments: string, int
            //     Type Parameters: <None>
            //     DeclaringType: OuterClass
            //   OuterClass:
            //     Type Arguments: <None>
            //     Type Parameters: 1
            else if (typeReference.HasGenericParameters && genericTypeArguments.Any())
            {
                return GetGenericTypeInstanceId(typeReference, genericTypeArguments);

            }
            // Unbound generic type, e.g. 
            // class Foo<T> 
            // { }        
            else if (typeReference.HasGenericParameters)
            {
                return GetGenericTypeId(typeReference);
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
            // reference type ('ref', 'out' and 'in' parameters)
            else if (typeReference is ByReferenceType byReferenceType)
            {
                return new ByReferenceTypeId(byReferenceType.ElementType.ToTypeId());
            }
            // type is a "normal", non-generic type
            else
            {
                // nested type
                if (typeReference.IsNested)
                {
                    return new SimpleTypeId(typeReference.DeclaringType.ToTypeId(), typeReference.Name);
                }
                // top-level / non-nested type
                else
                {
                    return new SimpleTypeId(new NamespaceId(typeReference.Namespace), typeReference.Name);
                }
            }
        }


        private static TypeId GetGenericTypeInstanceId(GenericInstanceType typeReference)
        {
            return GetGenericTypeInstanceId(typeReference, typeReference.GenericArguments);
        }

        private static TypeId GetGenericTypeInstanceId(TypeReference typeReference, IEnumerable<TypeReference> genericTypeArguments)
        {
            if (typeReference.IsNested)
            {
                // Handling of nested generic types:
                // For e.g. the type 'OuterClass<string>.NestedClass<int>', the type reference looks like this
                //
                //   NestedClass`1: 
                //     Type Arguments: string, int
                //     Type Parameters: <None>
                //     DeclaringType ──────────┐
                //                             │
                //   OuterClass`1:  <──────────┘
                //     Type Arguments: <None>
                //     Type Parameters: 1
                //
                // In that case, get the required arguments for the declaring type and construct a type id for it
                // The remaining type arguments are used to build the type id for the current type
                // If *all* the arguments are required for the declaring type, return a non-generic type.
                // This is valid when a non-generic type is nested inside a generic type, e.g. 'OuterClass<string>.InnerClass'
                //
                var myTypeArgs = genericTypeArguments.Skip(typeReference.DeclaringType.GenericParameters.Count).ToArray();
                var declaringTypeArgs = genericTypeArguments.Take(typeReference.DeclaringType.GenericParameters.Count);

                var declaringTypeId = typeReference.DeclaringType.ToTypeId(declaringTypeArgs);

                // for nested generic instance types, the type has all the type arguments
                // including the arguments for the declaring types

                if (myTypeArgs.Length == 0)
                {
                    return new SimpleTypeId(declaringTypeId, typeReference.Name);
                }
                else
                {
                    // remove the number of type parameters from the name
                    var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                    var typeArguments = myTypeArgs
                        .Select(x => x.ToTypeId())
                        .ToArray();

                    return new GenericTypeInstanceId(
                        declaringTypeId,
                        name,
                        typeArguments);
                }
            }
            else
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                var typeArguments = genericTypeArguments
                    .Select(x => x.ToTypeId())
                    .ToArray();

                return new GenericTypeInstanceId(
                    new NamespaceId(typeReference.Namespace),
                    name,
                    typeArguments);
            }
        }

        private static TypeId GetGenericTypeId(TypeReference typeReference)
        {
            // if the type is nested and has generic parameters,
            // no all parameters are parameters of the type but might also be parameters
            // of the declaring type,
            // e.g.
            //
            //  class Class1<T>
            //  {
            //      class Class2
            //      {
            //      }
            //  }
            //
            // In this case, Class2 would also have a generic type parameter
            //
            if (typeReference.IsNested)
            {
                // get the id of the declaring type
                var declaringType = typeReference.DeclaringType.ToTypeId();

                // determine which type parameter are the type's parameters
                // and which parameters are parameters of the declaring types
                var declaringTypeGenericParamters = typeReference.DeclaringType
                    .GenericParameters
                    .Select(x => x.Name)
                    .ToArray();

                var genericParameters = typeReference
                    .GenericParameters
                    .Select(x => x.Name)
                    .Except(declaringTypeGenericParamters)
                    .ToArray();

                // if all type parameters are declared in the declaring type,
                // return a non-generic type
                if (genericParameters.Length == 0)
                {
                    return new SimpleTypeId(declaringType, typeReference.Name);
                }
                else
                {
                    // remove the number of type parameters from the name
                    var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));
                    return new GenericTypeId(declaringType, name, genericParameters.Length, genericParameters);
                }
            }
            else
            {
                // remove the number of type parameters from the name
                var name = typeReference.Name.Substring(0, typeReference.Name.LastIndexOf('`'));

                // get the names of the type parameters
                // so the GenericTypeId's DisplayName matches
                // the definition
                var typeParameterNames = typeReference.GenericParameters.Select(x => x.Name).ToArray();

                return new GenericTypeId(new NamespaceId(typeReference.Namespace), name, typeReference.GenericParameters.Count, typeParameterNames);
            }
        }
    }
}
