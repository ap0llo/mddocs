using System;
using MdDoc.Model;

namespace MdDoc.Pages
{
    class CompositeLinkProvider : ILinkProvider
    {
        private readonly ILinkProvider[] m_InnerProviders;

        public CompositeLinkProvider(params ILinkProvider[] innerProviders)
        {
            m_InnerProviders = innerProviders ?? throw new ArgumentNullException(nameof(innerProviders));
        }

        public bool TryGetLink(MemberId id, out string link)
        {
            foreach(var provider in m_InnerProviders)
            {
                if (provider.TryGetLink(id, out link))
                    return true;
            }

            link = default;
            return false;
        }
    }
}
