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

            // public
            definitionBuilder.Append("public ");

            // statoco
            if(!property.HasThis)
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

            // public
            if(field.IsPublic)
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

            // constructor
            if (method.IsConstructor)
            {
                // omit the "public" moifier for static class initializers
                if(method.IsStatic)
                {
                    definitionBuilder.Append("static ");

                }
                else if(method.IsPublic)
                {
                    definitionBuilder.Append("public ");

                }

                // no return type

                // metho name is the name of the type
                definitionBuilder.Append(method.DeclaringType.Name);
            }
            // nn-constructor method
            else
            {
                // public
                if (method.IsPublic)
                {
                    definitionBuilder.Append("public ");
                }

                // static
                if (method.IsStatic)
                {
                    definitionBuilder.Append("static ");
                }

                // return type and method name
                definitionBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
                definitionBuilder.Append(" ");
                definitionBuilder.Append(method.Name);
            }

            // type parameters
            if(method.GenericParameters.Count > 0)
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
            definitionBuilder.AppendJoin(
                ", ",
                method.Parameters.Select(p => $"{p.ParameterType.ToTypeId().DisplayName} {p.Name}")
            );
            definitionBuilder.Append(");");

            return definitionBuilder.ToString();
        }

        //TODO: Classes, interfaces, enum, structs
        //TODO: Events        
    }
}
