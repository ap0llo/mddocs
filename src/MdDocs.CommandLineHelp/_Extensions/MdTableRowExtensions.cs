using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class MdTableRowExtensions
    {
        public static void AddIf(this MdTableRow row, bool condition, MdSpan cell)
        {
            if (condition)
                row.Add(cell);
        }
    }
}
