using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class XmlDocsDocument
    {
        private readonly XDocument m_Document;


        public IReadOnlyCollection<XmlDocsMember> Members { get; }


        public XmlDocsDocument(string path) : this(XDocument.Load(path))
        { }

        public XmlDocsDocument(XDocument document)
        {            
            if (document.Root.Name != "doc")
                throw new InvalidXmlDocsException($"Unrecognized root element name '{document.Root.Name}', expected 'doc'");

            m_Document = document ?? throw new ArgumentNullException(nameof(document));

            var membersElement = document.Root.Element("members");
            Members = membersElement == null
                ? Array.Empty<XmlDocsMember>()
                : membersElement.Elements("member").Select(x => new XmlDocsMember(x)).ToArray();
        }        

    }
}
