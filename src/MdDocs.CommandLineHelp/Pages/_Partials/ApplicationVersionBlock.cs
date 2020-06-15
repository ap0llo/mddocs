using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model2;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class ApplicationVersionBlock : MdPartial
    {
        private readonly ApplicationDocumentation m_Model;

        public ApplicationVersionBlock(ApplicationDocumentation model)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        protected override MdBlock ConvertToBlock()
        {
            if (String.IsNullOrEmpty(m_Model.Version))
            {
                return MdEmptyBlock.Instance;
            }
            else
            {
                return new MdParagraph(new MdStrongEmphasisSpan("Version:"), " ", m_Model.Version);
            }
        }
    }
}
