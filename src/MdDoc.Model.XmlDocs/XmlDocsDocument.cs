using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class XmlDocsDocument
    {
        public IReadOnlyCollection<XmlDocsMember> Members { get; }

        internal XDocument Xml { get; }

        public XmlDocsDocument(string path) : this(XDocument.Load(path))
        { }

        public XmlDocsDocument(XDocument xml)
        {            
            if (xml.Root.Name != "doc")
                throw new InvalidXmlDocsException($"Unrecognized root element name '{xml.Root.Name}', expected 'doc'");

            Xml = xml ?? throw new ArgumentNullException(nameof(xml));

            var membersElement = xml.Root.Element("members");
            Members = membersElement == null
                ? Array.Empty<XmlDocsMember>()
                : membersElement.Elements("member").Select(x => new XmlDocsMember(x)).ToArray();
        }        

    }
}
