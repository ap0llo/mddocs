#nullable disable

using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.Common.Pages
{
    public abstract class MdPartial
    {
        protected abstract MdBlock ConvertToBlock();


        public static implicit operator MdBlock(MdPartial partial) => partial?.ConvertToBlock();

    }
}
