using System;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs
{
    public class XmlDocsMember
    {
        private readonly XElement m_Element;


        public string Name { get; }


        public XmlDocsMember(XElement element)
        {
            if (element.Name != "member")
                throw new InvalidXmlDocsException($"Unrecognized element name '{element.Name}', expected 'member'");

            var nameAttribute = element.Attribute("name");
            if (nameAttribute == null)
                throw new InvalidXmlDocsException("Attribute 'name' is missing");

            if (String.IsNullOrEmpty(nameAttribute.Value))
                throw new InvalidXmlDocsException("Value of attribute 'name' must not be empty");

            m_Element = element ?? throw new ArgumentNullException(nameof(element));
            Name = nameAttribute.Value;
        }
    }
}
