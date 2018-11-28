using Mono.Cecil;
using System;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class PropertyDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public TypeName Type { get; }

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
                definitionBuilder.Append(new TypeName(Definition.PropertyType));
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
                        Definition.Parameters.Select(x => $"{new TypeName(x.ParameterType)} {x.Name}")
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
            Type = new TypeName(definition.PropertyType);
        }        

    }
}
