using System.Collections.Generic;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

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
