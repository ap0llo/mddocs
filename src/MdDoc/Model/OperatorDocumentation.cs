using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class OperatorDocumentation : MemberDocumentation
    {
        private readonly DocumentationContext m_Context;

        public OperatorKind Kind { get; }

        public IReadOnlyCollection<MethodDefinition> Definitions { get; }


        public OperatorDocumentation(TypeDocumentation typeDocumentation, DocumentationContext context, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definitions = definitions?.ToArray() ?? throw new ArgumentNullException(nameof(definitions));

            var definitionList = new List<MethodDefinition>();

            OperatorKind? previousKind = null;
            foreach (var definition in definitions)
            {
                var kind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method '{definition.Name}' is not a operator overload");
                definitionList.Add(definition);

                if(previousKind.HasValue && previousKind.Value != kind)
                {
                    throw new ArgumentException("Cannot combine overloads of different operators");
                }
                previousKind = kind;
                Kind = kind;
            }            
        }
        
    }
}
