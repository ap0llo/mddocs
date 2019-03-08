using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference
{
    internal static class ListExtensions
    {
        [Obsolete("ToReadOnly()is obsolete. Use 'AsReadOnlyList()' from package Grynwald.Utilities instead")]
        public static IReadOnlyList<T> ToReadOnly<T>(this List<T> list) => list;
    }
}
