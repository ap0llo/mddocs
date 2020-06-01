using System;
using System.Text;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal struct NamespaceIdBuilder
    {
        private StringBuilder m_NameBuilder;

        public void AddNameSegment(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            m_NameBuilder = m_NameBuilder ?? new StringBuilder();

            if (m_NameBuilder.Length > 0)
            {
                m_NameBuilder.Append(".");
            }

            m_NameBuilder.Append(name);
        }

        public NamespaceId ToNamespaceId()
        {
            if (m_NameBuilder == null)
            {
                return NamespaceId.GlobalNamespace;
            }
            else
            {
                return new NamespaceId(m_NameBuilder.ToString());
            }
        }
    }
}
