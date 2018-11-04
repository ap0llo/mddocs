using Grynwald.MarkdownGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    abstract class MemberPage : PageBase
    {
        protected abstract TypeReference DeclaringType { get; }

        public MemberPage(DocumentationContext context, PathProvider pathProvider) : base(context, pathProvider)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("DeclaringType:"), " ", GetTypeNameSpan(DeclaringType)
            ));
        }        
    }
}
