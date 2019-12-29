#nullable disable

using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class CompositeLinkProvider : ILinkProvider
    {
        private readonly ILinkProvider[] m_InnerProviders;


        public CompositeLinkProvider(params ILinkProvider[] innerProviders)
        {
            m_InnerProviders = innerProviders ?? throw new ArgumentNullException(nameof(innerProviders));
        }

        
        public bool TryGetLink(IDocument from, MemberId id, out Link link)
        {
            foreach (var provider in m_InnerProviders)
            {
                if (provider.TryGetLink(from, id, out link))
                    return true;
            }

            link = default;
            return false;
        }
    }
}
