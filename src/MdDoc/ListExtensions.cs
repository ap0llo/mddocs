using System.Collections.Generic;

namespace MdDoc
{
    internal static class ListExtensions
    {
        public static IReadOnlyList<T> ToReadOnly<T>(this List<T> list) => list;
    }
}
