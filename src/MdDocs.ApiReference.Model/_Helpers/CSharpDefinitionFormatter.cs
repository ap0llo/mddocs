using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Text;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Helper class to create C# from Mono.Cecil definitions.
    /// </summary>
    /// <example>
    /// For a get-only string property called "MyProperty", <see cref="GetDefinition(PropertyDefinition)" /> would return
    /// <code language="csharp">
    /// public string MyProperty { get; }
    /// </code>
    /// </example>
    internal static class CSharpDefinitionFormatter
    {
        /// <summary>
        /// Gets the C# code defining the specified property.
        /// </summary>
        public static string GetDefinition(PropertyDefinition property)
        {
            var definitionBuilder = new StringBuilder();

            AppendCustomAttributes(definitionBuilder, property.CustomAttributes);

            // "public"
            definitionBuilder.Append("public ");

            // "static"
            if (property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true)
            {
                definitionBuilder.Append("static ");
            }

            // type
            definitionBuilder.Append(GetDisplayName(property.PropertyType));
            definitionBuilder.Append(" ");

            // property name or "this" if the property is an indexer
            if (property.HasParameters)
            {
                definitionBuilder.Append("this");

                // parameters (for indexers)
                definitionBuilder.Append("[");
                definitionBuilder.AppendJoin(
                    ", ",
                    property.Parameters.Select(GetDefinition)
                );
                definitionBuilder.Append("]");
            }
            else
            {
                definitionBuilder.Append(property.Name);
            }

            definitionBuilder.Append(" ");

            // getter and setter
            var hasGetter = property.GetMethod?.IsPublic == true;
            var hasSetter = property.SetMethod?.IsPublic == true;

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

        /// <summary>
        /// Gets the C# code defining the specified field.
        /// </summary>
        public static string GetDefinition(FieldDefinition field)
        {
            var definitionBuilder = new StringBuilder();

            AppendCustomAttributes(definitionBuilder, field.CustomAttributes);

            // "public"
            if (field.IsPublic)
            {
                definitionBuilder.Append("public ");
            }

            // "static"
            if (field.IsStatic && !field.HasConstant)
            {
                definitionBuilder.Append("static ");
            }

            // "const"
            if (field.HasConstant)
            {
                definitionBuilder.Append("const ");
            }

            // "readonly"
            if (field.IsInitOnly)
            {
                definitionBuilder.Append("readonly ");
            }

            // field type
            definitionBuilder.Append(GetDisplayName(field.FieldType));
            definitionBuilder.Append(" ");

            // field name
            definitionBuilder.Append(field.Name);

            definitionBuilder.Append(";");

            return definitionBuilder.ToString();
        }

        /// <summary>
        /// Gets the C# code defining the specified method.
        /// </summary>
        public static string GetDefinition(MethodDefinition method)
        {
            var definitionBuilder = new StringBuilder();

            // attributes
            AppendCustomAttributes(definitionBuilder, method.GetCustomAttributes());

            // method is constructor
            if (method.IsConstructor)
            {
                // omit the "public" modifier for static class initializers
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
                var methodName = method.DeclaringType.Name;

                if (method.DeclaringType.HasGenericParameters)
                {
                    // remove number of type parameters from type name
                    methodName = methodName.Substring(0, methodName.LastIndexOf("`"));
                }

                definitionBuilder.Append(methodName);
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
                    // implicit and explicit conversion operators do not have a return type in the signature
                    // instead, the return type is the operator
                    // e.g. public static explicit operator int(myType x);
                    if (operatorKind == OperatorKind.Implicit)
                    {
                        definitionBuilder.Append("implicit operator ");
                        definitionBuilder.Append(GetDisplayName(method.ReturnType));
                    }
                    else if (operatorKind == OperatorKind.Explicit)
                    {
                        definitionBuilder.Append("explicit operator ");
                        definitionBuilder.Append(GetDisplayName(method.ReturnType));
                    }
                    else
                    {
                        // return type
                        definitionBuilder.Append(GetDisplayName(method.ReturnType));
                        definitionBuilder.Append(" ");

                        // operator
                        definitionBuilder.Append("operator ");
                        definitionBuilder.Append(GetOperatorString(operatorKind));
                    }
                }
                else
                {
                    // return type
                    definitionBuilder.Append(GetDisplayName(method.ReturnType));
                    definitionBuilder.Append(" ");

                    // method name
                    definitionBuilder.Append(method.Name);
                }
            }

            // type parameters
            if (method.HasGenericParameters)
            {
                AppendTypeParameters(definitionBuilder, method.GenericParameters);
            }

            // method parameters
            definitionBuilder.Append("(");
            if (method.IsExtension())
            {
                definitionBuilder.Append("this ");
            }

            definitionBuilder.AppendJoin(
                ", ",
                method.Parameters.Select(GetDefinition)
            );
            definitionBuilder.Append(");");

            return definitionBuilder.ToString();
        }

        /// <summary>
        /// Gets the C# code defining the specified event.
        /// </summary>
        public static string GetDefinition(EventDefinition @event)
        {
            var definitionBuilder = new StringBuilder();

            AppendCustomAttributes(definitionBuilder, @event.CustomAttributes);

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
            definitionBuilder.Append(GetDisplayName(@event.EventType));
            definitionBuilder.Append(" ");

            // event name
            definitionBuilder.Append(@event.Name);
            definitionBuilder.Append(";");

            return definitionBuilder.ToString();
        }

        /// <summary>
        /// Gets the C# code defining the specified type.
        /// </summary>
        public static string GetDefinition(TypeDefinition type)
        {
            var definitionBuilder = new StringBuilder();

            var typeKind = type.Kind();

            AppendCustomAttributes(definitionBuilder, type.GetCustomAttributes());
            AppendTypeModifiers(definitionBuilder, type, typeKind);

            // "class" / "interface" / "struct" / "enum"
            definitionBuilder.Append(typeKind.ToString().ToLower());
            definitionBuilder.Append(" ");

            // class name and type parameters
            if (type.HasGenericParameters)
            {
                // remove number of type parameters from type name
                var name = type.Name.Substring(0, type.Name.LastIndexOf("`"));
                definitionBuilder.Append(name);
                AppendTypeParameters(definitionBuilder, type.GenericParameters);
            }
            else
            {
                definitionBuilder.Append(type.Name);
            }

            // base type and interface implementations
            AppendBaseTypes(type, typeKind, definitionBuilder);

            AppendDefinitionBody(definitionBuilder, type, typeKind);

            return definitionBuilder.ToString();
        }

   
        private static void AppendCustomAttributes(StringBuilder definitionBuilder, IEnumerable<CustomAttribute> customAttributes, bool singleLine = false)
        {
            foreach (var attribute in customAttributes)
            {
                var attributeName = GetDisplayName(attribute.AttributeType);

                // remove the "Attribute" suffix from the attribute type's name
                if (attributeName.EndsWith("Attribute"))
                    attributeName = attributeName.Substring(0, attributeName.Length - "Attribute".Length);

                definitionBuilder.Append("[");
                definitionBuilder.Append(attributeName);

                // if there are any parameters or properties defined, append them as well)
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

                if(!singleLine)
                {
                    definitionBuilder.Append("\r\n");
                }
            }
        }

        private static void AppendTypeParameters(StringBuilder definitionBuilder, IEnumerable<GenericParameter> genericParameters)
        {
            //TODO: parameter constraints

            definitionBuilder.Append("<");

            var isFirst = true;
            foreach (var parameter in genericParameters)
            {
                if (!isFirst)
                {
                    definitionBuilder.Append(", ");
                }

                isFirst = false;


                if (parameter.IsCovariant)
                {
                    definitionBuilder.Append("out ");
                }

                if (parameter.IsContravariant)
                {
                    definitionBuilder.Append("in ");
                }

                definitionBuilder.Append(parameter.Name);
            }

            definitionBuilder.Append(">");
        }

        private static void AppendTypeModifiers(StringBuilder definitionBuilder, TypeDefinition type, TypeKind typeKind)
        {
            // "public"
            if (type.IsPublic)
            {
                definitionBuilder.Append("public ");
            }

            switch (typeKind)
            {
                case TypeKind.Class:

                    // "static"
                    //
                    // Check if class is static, however there are no static classes on IL level
                    // static classes are both abstract and sealed which cannot be declared in C#,
                    // so a abstract sealed class is assumed to be static
                    var isStatic = type.IsSealed && type.IsAbstract;
                    if (isStatic)
                    {
                        definitionBuilder.Append("static ");
                    }
                    // ignore "abstract" and "sealed" modifiers for static classes
                    else
                    {
                        // "abstract"
                        if (type.IsAbstract)
                        {
                            definitionBuilder.Append("abstract ");
                        }

                        if (type.IsSealed)
                        {
                            definitionBuilder.Append("sealed ");
                        }
                    }
                    break;

                case TypeKind.Struct:
                    if (type.CustomAttributes.Any(a => a.AttributeType.FullName == Constants.IsReadOnlyAttributeFullName))
                    {
                        definitionBuilder.Append("readonly ");
                    }
                    break;

            }
        }

        private static void AppendBaseTypes(TypeDefinition type, TypeKind typeKind, StringBuilder definitionBuilder)
        {
            if (typeKind == TypeKind.Enum)
            {
                var underlyingType = type.Fields.Single(f => f.Name == "value__").FieldType;
                if (underlyingType?.FullName != Constants.Int32FullName)
                {
                    definitionBuilder.Append(" : ");
                    definitionBuilder.Append(GetDisplayName(underlyingType));
                }
            }
            else
            {
                // get the default (implicit) base type: "object" for classes, "System.ValueType" for structs
                // if the base type is the default type, the base type will not be explicitly included in the definition
                var defaultBaseType = typeKind == TypeKind.Struct
                    ? Constants.ValueTypeFullName
                    : (typeKind == TypeKind.Class ? Constants.ObjectFullName : "");

                if (type.HasInterfaces)
                {
                    definitionBuilder.Append(" : ");
                    if (type.BaseType != null && type.BaseType.FullName != defaultBaseType)
                    {
                        definitionBuilder.Append(GetDisplayName(type.BaseType));
                        definitionBuilder.Append(", ");
                    }
                    definitionBuilder.AppendJoin(
                        ", ",
                        type.Interfaces.Select(i => GetDisplayName(i.InterfaceType))
                    );
                }
                else if (type.BaseType != null && type.BaseType.FullName != defaultBaseType)
                {
                    definitionBuilder.Append(" : ");
                    definitionBuilder.Append(GetDisplayName(type.BaseType));
                }
            }
        }

        private static void AppendDefinitionBody(StringBuilder definitionBuilder, TypeDefinition type, TypeKind typeKind)
        {
            if (typeKind != TypeKind.Enum)
                return;

            var isFlagsEnum = IsFlagsEnum(type);

            definitionBuilder.AppendLine();
            definitionBuilder.AppendLine("{");
            definitionBuilder.AppendJoin(
                ",\r\n",
                type.GetEnumValues().Select(x => $"    {x.name} = {(isFlagsEnum ? "0x" + x.value.ToString("X") : x.value.ToString())}")
            );
            definitionBuilder.AppendLine();
            definitionBuilder.AppendLine("}");
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

        private static string GetLiteral(CustomAttributeArgument attributeArgument) => GetLiteral(attributeArgument.Type, attributeArgument.Value);

        private static string GetLiteral(TypeReference typeReference, object value)
        {
            var typeDefinition = typeReference.Resolve();

            // string => put in quotation marks
            if (typeReference.FullName == Constants.StringFullName)
            {
                if(value is null)
                {
                    return "null";
                }
                else
                {
                    return $"\"{value}\"";
                }
            }
            // special handling for enums
            else if (typeDefinition != null && typeDefinition.Kind() == TypeKind.Enum)
            {
                // get the arguments value and all possible values for the enum
                var intValue = Convert.ToInt64(value);
                var values = typeDefinition.GetEnumValues();

                // get the friendly name for the enum
                var enumName = GetDisplayName(typeDefinition);

                // for "Flags" enums, return all values
                if (IsFlagsEnum(typeDefinition))
                {
                    var builder = new StringBuilder();

                    foreach (var (name, enumValue) in values)
                    {
                        if ((intValue & enumValue) != 0)
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
                else if (values.Any(x => x.value == intValue))
                {
                    return $"{enumName}.{values.First(x => x.value == intValue).name}";
                }
                // on error (e.g. a value not defined in the enum), just return the plain value
                else
                {
                    return value.ToString();
                }
            }
            // otherwise: convert value to string
            else
            {
                return value.ToString();
            }
        }

        private static bool IsFlagsEnum(TypeDefinition type) => type.CustomAttributes.Any(a => a.AttributeType.FullName == Constants.FlagsAttributeFullName);

        private static string GetDisplayName(TypeReference typeReference)
        {
            return typeReference.ToTypeId().DisplayName;
        }

        private static string GetDefinition(ParameterDefinition parameter)
        {
            var definitionBuilder = new StringBuilder();
            var parameterType = parameter.ParameterType;

            // If the parameter is optional, but does not have a default value
            // display it as [Optional] attribute at the before the parameter.
            // If the parameter is optional AND has a default parameter,
            // render it as " = <VALUE>" after the parameter (see below).
            if (parameter.Attributes.HasFlag(ParameterAttributes.Optional) && !parameter.Attributes.HasFlag(ParameterAttributes.HasDefault))
            {
                definitionBuilder.Append("[Optional]");
            }

            AppendCustomAttributes(definitionBuilder, parameter.GetCustomAttributes(), singleLine: true);

            // special handling for 'out' and 'ref' parameters
            // do not use the type's actual display name, but add the modified before the parameter
            // and use the by-reference type's element type,
            // i.e. display "ref string parameter" instead of "string& parameter"
            if (parameterType is ByReferenceType byReferenceType)
            {
                parameterType = byReferenceType.ElementType;
                if(parameter.Attributes.HasFlag(ParameterAttributes.Out))
                {
                    definitionBuilder.Append("out ");
                }
                else if (parameter.Attributes.HasFlag(ParameterAttributes.In))
                {
                    definitionBuilder.Append("in ");
                }
                else
                {
                    definitionBuilder.Append("ref ");
                }
            }

            // add parameter type
            definitionBuilder.Append(GetDisplayName(parameterType));
            definitionBuilder.Append(" ");

            // add parameter name
            definitionBuilder.Append(parameter.Name);
            
            // if parameter has a default value, include it in the definition
            if (parameter.Attributes.HasFlag(ParameterAttributes.Optional) &&
                parameter.Attributes.HasFlag(ParameterAttributes.HasDefault))
            {
                definitionBuilder.Append(" = ");
                definitionBuilder.Append(GetLiteral(parameter.ParameterType, parameter.Constant));                
            }

            return definitionBuilder.ToString();
        }
    }
}
