using System.Text;
using System.Linq;
using Mono.Cecil;

namespace MdDoc
{
    class XmlDocNameMapper
    {        
        public string GetXmlDocName(TypeReference type) => $"T:{type.FullName}";

        public string GetXmlDocName(MethodDefinition method)
        {
            var stringBuilder = new StringBuilder();

            // methods are prefixed with "M:", followed by the type name 
            stringBuilder.Append("M:");
            stringBuilder.Append(method.DeclaringType.FullName);
            stringBuilder.Append(".");

            // the name of the method is "#ctor" if it is a constructor
            if (method.IsConstructor)
            {
                stringBuilder.Append("#ctor");
            }
            else
            {
                stringBuilder.Append(method.Name);
            }
        
            //If the method has generic parameters, the number of parameters is appended to the name
            if(method.HasGenericParameters)
            {
                stringBuilder.Append("``");
                stringBuilder.Append(method.GenericParameters.Count);
            }

            // Append parameters
            if (method.HasParameters)
            {
                var parameterNames = method.Parameters.Select(p => GetSerializedTypeName(p.ParameterType));

                stringBuilder.Append("(");            
                stringBuilder.AppendJoin(",", parameterNames);               
                stringBuilder.Append(")");
            }

            // if the method overloads the conversion operator, 
            // the return type is appended to the name (separated by '~')
            if (method.IsSpecialName)
            {
                if(method.Name == "op_Implicit" || method.Name == "op_Explicit")
                {
                    stringBuilder.Append("~");
                    stringBuilder.Append(GetSerializedTypeName(method.ReturnType));
                }
            }

            return stringBuilder.ToString();
        }

        public string GetXmlDocName(PropertyDefinition property)
        {
            var stringBuilder = new StringBuilder();

            // properties are prefixed with "P:", followed by the type name 
            stringBuilder.Append("P:");
            stringBuilder.Append(property.DeclaringType.FullName);
            stringBuilder.Append(".");

            // append property name
            stringBuilder.Append(property.Name);

            // indexers are implemented as properties with parameters
            if (property.HasParameters)
            {
                var parameterNames = property.Parameters.Select(p => GetSerializedTypeName(p.ParameterType));

                stringBuilder.Append("(");
                stringBuilder.AppendJoin(",", parameterNames);
                stringBuilder.Append(")");
            }

            return stringBuilder.ToString();
        }

        public string GetXmlDocName(FieldDefinition field)
        {            
            // methods are prefixed with "F:", followed by the type and field names
            return $"F:{field.DeclaringType.FullName}.{field.Name}";            
        }


        private string GetSerializedTypeName(TypeReference type)
        {      
            //
            // Generic parameters are serialized as the index of the parameter, prefixed by ` or ``
            // - '`' refers to a generic parameter of a type
            // - '``' refers to a generic parameter of a method
            //
            // Examples:
            //  - A type that is the first type parameter of a type is serialized as '`0'
            //  - A type that is the second type parameter of a method is serialized as '``1'
            //
            if (type.IsGenericParameter)
            {
                var genericParameter = (GenericParameter)type;

                if (genericParameter.DeclaringMethod != null)
                {
                    return $"``{genericParameter.DeclaringMethod.GenericParameters.IndexOf(genericParameter)}";
                    
                }
                else
                {
                    return $"`{genericParameter.DeclaringType.GenericParameters.IndexOf(genericParameter)}";
                }
            }

            //
            // Generic arguments, e.g. IEnumerable<string> 
            // are serialized as TYPENAME{ARGUMENT1,ARGUMENT2}
            //
            if (type is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                // in XML docs, the number of generic arguments is not part of the type name.
                // Thus, we need to remove the ` suffix
                var typeName = genericType.Name.Replace($"`{genericType.GenericArguments.Count}", "");

                var stringBuilder = new StringBuilder();

                // append name of the type (namespace + name)
                stringBuilder.Append(type.Namespace);
                stringBuilder.Append(".");
                stringBuilder.Append(typeName);

                stringBuilder.Append("{");
                stringBuilder.AppendJoin(",", genericType.GenericArguments.Select(GetSerializedTypeName));
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }

            // if no generics are involved, simply return the type name
            return type.FullName;
        }        
    }
}
