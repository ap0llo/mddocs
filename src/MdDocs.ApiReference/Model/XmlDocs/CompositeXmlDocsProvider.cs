using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal sealed class CompositeXmlDocsProvider : IXmlDocsProvider
    {
        private readonly List<IXmlDocsProvider> m_InnerProviders = new();


        public CompositeXmlDocsProvider()
        { }


        public MemberElement? TryGetDocumentationComments(MemberId id)
        {
            foreach (var provider in m_InnerProviders)
            {
                var documentationComments = provider.TryGetDocumentationComments(id);

                if (documentationComments is not null)
                    return documentationComments;
            }
            return null;
        }

        public void Add(IXmlDocsProvider innerProvider) => m_InnerProviders.Add(innerProvider);
    }
}
