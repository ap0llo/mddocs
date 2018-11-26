using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class PropertyDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public TypeReference Type => Definition.PropertyType;

        // Indexeres are modeled as properties with parameters
        public bool IsIndexer => Definition.HasParameters;

        public PropertyDefinition Definition { get; }

        public string CSharpDefinition
        {
            get
            {
                var hasGetter = Definition.GetMethod?.IsPublic == true;
                var hasSetter = Definition.SetMethod?.IsPublic == true;

                var definitionBuilder = new StringBuilder();
                definitionBuilder.Append("public ");
                definitionBuilder.Append(GetTypeName(Definition.PropertyType));
                definitionBuilder.Append(" ");

                if(Definition.HasParameters)
                    definitionBuilder.Append("this");
                else
                    definitionBuilder.Append(Name);
                
                if(IsIndexer)
                {
                    definitionBuilder.Append("[");

                    definitionBuilder.AppendJoin(
                        ", ",
                        Definition.Parameters.Select(x => $"{GetTypeName(x.ParameterType)} {x.Name}")
                    );

                    definitionBuilder.Append("]");
                }


                definitionBuilder.Append(" ");
                definitionBuilder.Append("{ ");

                if (hasGetter)
                    definitionBuilder.Append("get;");


                if(hasSetter)
                {
                    if(hasGetter)
                        definitionBuilder.Append(" ");

                    definitionBuilder.Append("set;");
                }

                definitionBuilder.Append(" }");

                return definitionBuilder.ToString();
            }
        }


        public PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }        


        private string GetTypeName(TypeReference type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return "bool";
                case "System.Byte":
                    return "byte";
                case "System.SByte":
                    return "sbyte";
                case "System.Char":
                    return "char";
                case "System.Decimal":
                    return "decimal";
                case "System.Double":
                    return "double";
                case "System.Single":
                    return "float";
                case "System.Int32":
                    return "int";
                case "System.UInt32":
                    return "uint";
                case "System.Int64":
                    return "long";
                case "System.UInt64":
                    return "ulong";
                case "System.Object":
                    return "object";
                case "System.Int16":
                    return "short";
                case "System.UInt16":
                    return "ushort";
                case "System.String":
                    return "string";
                default:
                    return type.Name;
            }

        }

    }
}
