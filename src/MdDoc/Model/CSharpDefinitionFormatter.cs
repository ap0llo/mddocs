﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model
{
    static class CSharpDefinitionFormatter
    {
        public static string GetDefinition(PropertyDefinition property)
        {
            var hasGetter = property.GetMethod?.IsPublic == true;
            var hasSetter = property.SetMethod?.IsPublic == true;

            var definitionBuilder = new StringBuilder();

            AppendAttributes(definitionBuilder, property.CustomAttributes);

            // public
            definitionBuilder.Append("public ");

            // static
            if(property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true)
            {
                definitionBuilder.Append("static ");
            }

            // type
            definitionBuilder.Append(property.PropertyType.ToTypeId().DisplayName);
            definitionBuilder.Append(" ");

            // property name, "this" if the property is an indexer
            if (property.HasParameters)
            {
                definitionBuilder.Append("this");

                // parameters (for indexers)
                definitionBuilder.Append("[");
                definitionBuilder.AppendJoin(
                    ", ",
                    property.Parameters.Select(x => $"{x.ParameterType.ToTypeId().DisplayName} {x.Name}")
                );
                definitionBuilder.Append("]");
            }
            else
            {
                definitionBuilder.Append(property.Name);
            }

            definitionBuilder.Append(" ");

            // getter and setter
            definitionBuilder.Append("{ ");
            if (hasGetter)
            {
                definitionBuilder.Append("get;");
            }

            if (hasSetter)
            {
                if (hasGetter)
                    definitionBuilder.Append(" ");

                definitionBuilder.Append("set;");
            }
            definitionBuilder.Append(" }");

            return definitionBuilder.ToString();
        }

        public static string GetDefinition(FieldDefinition field)
        {            
            var definitionBuilder = new StringBuilder();

            AppendAttributes(definitionBuilder, field.CustomAttributes);

            // public
            if (field.IsPublic)
            {
                definitionBuilder.Append("public ");
            }

            // static
            if(field.IsStatic && !field.HasConstant)
            {
                definitionBuilder.Append("static ");
            }

            // const
            if(field.HasConstant)
            {
                definitionBuilder.Append("const ");
            }

            // readonly
            if(field.Attributes.HasFlag(FieldAttributes.InitOnly))
            {
                definitionBuilder.Append("readonly ");
            }

            // type
            definitionBuilder.Append(field.FieldType.ToTypeId().DisplayName);
            definitionBuilder.Append(" ");

            // name
            definitionBuilder.Append(field.Name);

            definitionBuilder.Append(";");

            return definitionBuilder.ToString();
        }
        
        public static string GetDefinition(MethodDefinition method)
        {
            var definitionBuilder = new StringBuilder();

            // attributes
            var customAttributes = method.CustomAttributes.Where(a => a.AttributeType.FullName != Constants.ExtensionAttributeFullName);
            AppendAttributes(definitionBuilder, customAttributes);

            // method is constructor
            if (method.IsConstructor)
            {
                // omit the "public" moifier for static class initializers
                if (method.IsStatic)
                {
                    definitionBuilder.Append("static ");

                }
                else if (method.IsPublic)
                {
                    definitionBuilder.Append("public ");
                }

                // no return type

                // method name is the name of the type
                definitionBuilder.Append(method.DeclaringType.Name);
            }
            // non-constructor method
            else
            {
                // "public"
                if (method.IsPublic)
                {
                    definitionBuilder.Append("public ");
                }

                // "static"
                if (method.IsStatic)
                {
                    definitionBuilder.Append("static ");
                }


                // check if method is operator
                if (method.IsOperator(out var operatorKind))
                {
                    // implict and explict conversion operators do not have a return type in the signature
                    // instead, the return type is the operator
                    // e.g. public static explicit operator int(myType x);
                    if (operatorKind == OperatorKind.Implicit)
                    {
                        definitionBuilder.Append("implicit operator ");
                        definitionBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
                    }
                    else if (operatorKind == OperatorKind.Explicit)
                    {
                        definitionBuilder.Append("explicit operator ");
                        definitionBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
                    }
                    else
                    {
                        // return type
                        definitionBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
                        definitionBuilder.Append(" ");

                        // operator
                        definitionBuilder.Append("operator ");
                        definitionBuilder.Append(GetOperatorString(operatorKind));
                    }
                }
                else
                {
                    // return type
                    definitionBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
                    definitionBuilder.Append(" ");

                    // method name
                    definitionBuilder.Append(method.Name);
                }
            }

            // type parameters
            if (method.GenericParameters.Count > 0)
            {
                definitionBuilder.Append("<");
                definitionBuilder.AppendJoin(
                    ", ",
                    method.GenericParameters.Select(p => p.Name)
                );
                definitionBuilder.Append(">");
            }

            // method parameters
            definitionBuilder.Append("(");
            if (method.IsExtensionMethod())
            {
                definitionBuilder.Append("this ");
            }

            definitionBuilder.AppendJoin(
                ", ",
                method.Parameters.Select(p => $"{p.ParameterType.ToTypeId().DisplayName} {p.Name}")
            );
            definitionBuilder.Append(");");

            return definitionBuilder.ToString();
        }
        
        public static string GetDefinition(EventDefinition @event)
        {
            var definitionBuilder = new StringBuilder();

            AppendAttributes(definitionBuilder, @event.CustomAttributes);

            // "public"
            if (@event.AddMethod?.IsPublic == true || @event.RemoveMethod?.IsPublic == true)
            {
                definitionBuilder.Append("public ");
            }

            // "static"
            if (@event.AddMethod?.IsStatic == true || @event.RemoveMethod?.IsStatic == true)
            {
                definitionBuilder.Append("static ");
            }

            // "event"
            definitionBuilder.Append("event ");

            // event type
            definitionBuilder.Append(@event.EventType.ToTypeId().DisplayName);
            definitionBuilder.Append(" ");

            // event name
            definitionBuilder.Append(@event.Name);
            definitionBuilder.Append(";");

            return definitionBuilder.ToString();
        }

        //TODO: Classes, interfaces, enum, structs        

        private static void AppendAttributes(StringBuilder definitionBuilder, IEnumerable<CustomAttribute> customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                var attributeName = attribute.AttributeType.ToTypeId().DisplayName;

                // remove the "Attribute" suffix from the attribute type's name
                if (attributeName.EndsWith("Attribute"))
                    attributeName = attributeName.Substring(0, attributeName.Length - "Attribute".Length);

                definitionBuilder.Append("[");
                definitionBuilder.Append(attributeName);

                // if there are any paramters or properties defined, append them as well)
                if (attribute.HasConstructorArguments || attribute.HasProperties)
                {
                    definitionBuilder.Append("(");
                    definitionBuilder.AppendJoin(", ",
                        Enumerable.Union(
                            attribute.ConstructorArguments.Select(GetLiteral),
                            attribute.Properties.Select(p => $"{p.Name} = {GetLiteral(p.Argument)}")
                        )
                    );
                    definitionBuilder.Append(")");
                }

                definitionBuilder.Append("]");
                definitionBuilder.Append("\r\n");
            }
        }

        private static string GetOperatorString(OperatorKind kind)
        {
            switch (kind)
            {
                case OperatorKind.UnaryPlus:
                case OperatorKind.Addition:
                    return "+";
                case OperatorKind.UnaryNegation:
                case OperatorKind.Subtraction:
                    return "-";
                case OperatorKind.LogicalNot:
                    return "!";
                case OperatorKind.OnesComplement:
                    return "~";
                case OperatorKind.Increment:
                    return "++";
                case OperatorKind.Decrement:
                    return "--";
                case OperatorKind.True:
                    return "true";
                case OperatorKind.False:
                    return "false";
                case OperatorKind.Multiply:
                    return "*";
                case OperatorKind.Division:
                    return "/";
                case OperatorKind.Modulus:
                    return "%";
                case OperatorKind.BitwiseAnd:
                    return "&";
                case OperatorKind.BitwiseOr:
                    return "|";
                case OperatorKind.LeftShift:
                    return "<<";                    
                case OperatorKind.RightShift:
                    return ">>";                    
                case OperatorKind.ExclusiveOr:
                    return "^";
                case OperatorKind.Equality:
                    return "==";
                case OperatorKind.Inequality:
                    return "!=";
                case OperatorKind.LessThan:
                    return "<";
                case OperatorKind.GreaterThan:
                    return ">";
                case OperatorKind.LessThanOrEqual:
                    return "<=";
                case OperatorKind.GreaterThanOrEqual:
                    return ">=";
                case OperatorKind.Implicit:
                case OperatorKind.Explicit:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        private static string GetLiteral(CustomAttributeArgument attributeArgument)
        {
            // string => put in quotation marks
            if (attributeArgument.Type.FullName == Constants.StringFullName)
            {
                return $"\"{attributeArgument.Value}\"";
            }
            // special handling for enums
            else if(IsEnum(attributeArgument.Type))
            {
                // get the definition of the enum
                var typeDefinition = attributeArgument.Type.Resolve();

                // get the arguments value and all possible values for the enum
                var intValue = Convert.ToInt64(attributeArgument.Value);
                var values = GetEnumValues(typeDefinition);

                // get the friendly name for the enum
                var enumName = typeDefinition.ToTypeId().DisplayName;

                // for "Flags" enums, return all values
                if (IsFlagsEnum(typeDefinition))
                {
                    var builder = new StringBuilder();

                    foreach (var (name, value) in values)
                    {
                        if ((intValue & value) != 0)
                        {
                            if (builder.Length > 0)
                                builder.Append(" | ");

                            builder.Append(enumName);
                            builder.Append(".");
                            builder.Append(name);
                        }
                    }

                    return builder.ToString();
                }
                // for "normal" enums, return the name of the value
                else if(values.Any(x => x.value == intValue))
                {
                    return $"{enumName}.{values.First(x => x.value == intValue).name}";
                }
                // on error (e.g. a value not defined in the enum), just return the plain value
                else
                {
                    return attributeArgument.Value.ToString();
                }
            }
            // otherwise: convert value to string
            else
            {
                return attributeArgument.Value.ToString();
            }
        }

        private static bool IsFlagsEnum(TypeDefinition type)
        {
            return type.HasCustomAttributes &&
                   type.CustomAttributes.Any(a => a.AttributeType.FullName == Constants.FlagsAttributeFullName);
        }

        private static bool IsEnum(TypeReference type)
        {
            var definition = type.Resolve();
            return definition != null &&
                   definition.Kind() == TypeKind.Enum;
        }

        private static (string name, long value)[] GetEnumValues(TypeDefinition definition)
        {
            return definition.Fields
                .Where(f => f.IsPublic && !f.IsSpecialName)               
                .Select(f => (f.Name, Convert.ToInt64(f.Constant))).ToArray();
        }
    }
}
