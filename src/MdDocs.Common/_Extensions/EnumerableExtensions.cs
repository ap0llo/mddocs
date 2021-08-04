using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TKey> DuplicatesBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector)
                // use .Skip(1).Any() instead of .Count() to avoid enumerating the entire group
                .Where(group => group.Skip(1).Any())
                .Select(x => x.Key);
        }

        public static IEnumerable<TKey> DuplicatesBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.GroupBy(keySelector, comparer)
                // use .Skip(1).Any() instead of .Count() to avoid enumerating the entire group
                .Where(group => group.Skip(1).Any())
                .Select(x => x.Key);
        }

    }
}
