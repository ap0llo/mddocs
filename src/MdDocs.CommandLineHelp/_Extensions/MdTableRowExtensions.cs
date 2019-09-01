using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class MdTableRowExtensions
    {
        public static MdTableRow AddIf(this MdTableRow row, bool condition, MdSpan cell)
        {
            if (condition)
            {
                row.Add(cell);
            }

            return row;
        }
    }
}
