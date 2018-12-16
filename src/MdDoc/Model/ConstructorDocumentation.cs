using MdDoc.Model.XmlDocs;
using Mono.Cecil;
using System.Collections.Generic;

namespace MdDoc.Model
{
    public class ConstructorDocumentation : MethodDocumentation
    {
        internal ConstructorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) 
            : base(typeDocumentation, definitions, xmlDocsProvider)
        {
        }
    }
}
