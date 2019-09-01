using System;
using System.Collections.Generic;
using System.Text;
using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal abstract class MdPartial
    {
        protected abstract MdBlock ConvertToBlock();


        public static implicit operator MdBlock(MdPartial partial) => partial?.ConvertToBlock();

    }
}
