using System;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs
{
    public class XmlDocsMember
    {
        public string Name { get; }

        public XmlDocsTextBlock Summary { get; }

        internal XElement Xml { get; }


        public XmlDocsMember(XElement xml)
        {
            if (xml.Name != "member")
                throw new InvalidXmlDocsException($"Unrecognized element name '{xml.Name}', expected 'member'");

            var nameAttribute = xml.Attribute("name");
            if (nameAttribute == null)
                throw new InvalidXmlDocsException("Attribute 'name' is missing");

            if (String.IsNullOrEmpty(nameAttribute.Value))
                throw new InvalidXmlDocsException("Value of attribute 'name' must not be empty");

            Xml = xml ?? throw new ArgumentNullException(nameof(xml));
            Name = nameAttribute.Value;
            

            foreach(var element in xml.Elements())
            {
                if (element.Name == "summary")
                    Summary = new XmlDocsTextBlock(element);
            }

        }
    }
}
