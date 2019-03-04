using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference
{
    internal static class ListExtensions
    {
        public static IReadOnlyList<T> ToReadOnly<T>(this List<T> list) => list;
    }
}
