using System;
using System.Linq;
using System.Text;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class PropertyDocumentation : MemberDocumentation
    {
        public MemberId MemberId { get; }

        public string Name => Definition.Name;

        public TypeId PropertyType { get; }

        // Indexeres are modeled as properties with parameters
        public bool IsIndexer => Definition.HasParameters;
        
        public string CSharpDefinition
        {
            get
            {
                var hasGetter = Definition.GetMethod?.IsPublic == true;
                var hasSetter = Definition.SetMethod?.IsPublic == true;

                var definitionBuilder = new StringBuilder();
                definitionBuilder.Append("public ");
                definitionBuilder.Append(Definition.PropertyType.ToTypeId().DisplayName);
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
                        Definition.Parameters.Select(x => $"{x.ParameterType.ToTypeId().DisplayName} {x.Name}")
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

        public SummaryElement Summary { get; }

        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            PropertyType = definition.PropertyType.ToTypeId();
            MemberId = definition.ToMemberId();
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
