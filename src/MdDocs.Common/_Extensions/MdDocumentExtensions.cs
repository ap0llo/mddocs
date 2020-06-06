using System;
using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.Common
{
    public static class MdDocumentExtensions
    {
        public static MdDocument Add(this MdDocument document, MdBlock block)
        {
            document.Root.Add(block);
            return document;
        }

        public static MdDocument AddIf(this MdDocument document, bool condition, MdBlock block)
        {
            if (condition)
            {
                document.Root.Add(block);
            }

            return document;
        }

        public static MdDocument AddIf(this MdDocument document, bool condition, Func<MdBlock> getBlock)
        {
            if (condition)
            {
                document.Root.Add(getBlock());
            }

            return document;
        }
    }
}
