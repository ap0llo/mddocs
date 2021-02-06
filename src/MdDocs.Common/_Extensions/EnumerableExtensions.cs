using System.Collections.Generic;

namespace Grynwald.MdDocs.Common
{
    // Adapted from https://khalidabuhakmeh.com/implement-kotlins-withindex-in-csharp
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int index, T value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                yield return new(index++, item);
            }
        }
    }
}
