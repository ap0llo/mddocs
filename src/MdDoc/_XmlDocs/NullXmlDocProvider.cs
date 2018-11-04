using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Grynwald.MarkdownGenerator;
using Mono.Cecil;

namespace MdDoc
{
    class NullXmlDocProvider : IXmlDocProvider
    {

        public static readonly IXmlDocProvider Instance = new NullXmlDocProvider();

        private NullXmlDocProvider()
        { }

        public MemberDocumentation TryGetDocumentation(MemberReference member) => MemberDocumentation.Empty;
    }
}
