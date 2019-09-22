using System;
using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class MdContainerBlockBaseExtensions
    {
        public static T AddIf<T>(this T containerBlock, bool condition, MdBlock block) where T : MdContainerBlockBase
        {
            if (condition)
            {
                containerBlock.Add(block);
            }

            return containerBlock;
        }

        public static T AddIf<T>(this T containerBlock, bool condition, Func<MdBlock> getBlock) where T : MdContainerBlockBase
        {
            if (condition)
            {
                containerBlock.Add(getBlock());
            }

            return containerBlock;
        }


        public static TBlock AddIf<TBlock, TParam>(this TBlock containerBlock, bool condition, Func<TParam, MdBlock> getBlock, TParam parameter) where TBlock : MdContainerBlockBase
        {
            if (condition)
            {
                containerBlock.Add(getBlock(parameter));
            }

            return containerBlock;
        }
    }
}
