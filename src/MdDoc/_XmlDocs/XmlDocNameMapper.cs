using System;
using System.Collections.Generic;
using Mono.Cecil;
using System.Text;
using System.Linq;

namespace MdDoc
{
    class XmlDocNameMapper
    {

        public string GetXmlDocName(TypeReference type)
        {
            return $"T:{type.FullName}";
        }

        public string GetXmlDocName(MethodDefinition method)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("M:");

            stringBuilder.Append(method.DeclaringType.FullName);
            stringBuilder.Append(".");

            if (method.IsConstructor)
            {
                stringBuilder.Append("#ctor");
            }
            else
            {
                stringBuilder.Append(method.Name);
            }


            if(method.HasGenericParameters)
            {
                stringBuilder.Append("``");
                stringBuilder.Append(method.GenericParameters.Count);
            }


            if (method.HasParameters)
            {
                stringBuilder.Append("(");

                var first = true;
                foreach (var parameter in method.Parameters)
                {
                    if (!first)
                    {
                        stringBuilder.Append(",");
                    }                    
                    
                    stringBuilder.Append(GetSerializedTypeName(parameter.ParameterType));                    

                    first = false;

                }
                stringBuilder.Append(")");
            }

            
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

            stringBuilder.Append("P:");

            stringBuilder.Append(property.DeclaringType.FullName);
            stringBuilder.Append(".");
            stringBuilder.Append(property.Name);

            if (property.HasParameters)
            {
                stringBuilder.Append("(");

                var first = true;
                foreach (var parameter in property.Parameters)
                {
                    if (!first)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(GetSerializedTypeName(parameter.ParameterType));
                    first = false;

                }
                stringBuilder.Append(")");
            }

            return stringBuilder.ToString();
        }

        public string GetXmlDocName(FieldDefinition field)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("F:");

            stringBuilder.Append(field.DeclaringType.FullName);
            stringBuilder.Append(".");
            stringBuilder.Append(field.Name);

            return stringBuilder.ToString();
        }

        private string GetSerializedTypeName(TypeReference type)
        {

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


            if (type is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                var arguments = genericType.GenericArguments;

                var typeName = genericType.Name.Replace($"`{arguments.Count}", "");

                var stringBuilder = new StringBuilder();

                stringBuilder.Append(type.Namespace);
                stringBuilder.Append(".");
                stringBuilder.Append(typeName);
                stringBuilder.Append("{");

                var first = true;
                foreach (var typeArgument in arguments)
                {
                    if (!first)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(GetSerializedTypeName(typeArgument));
                    first = false;

                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();

            }

            return type.FullName;

        }
        
    }
}
