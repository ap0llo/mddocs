using System;
using System.Collections;
using System.Collections.Generic;

namespace MdDoc
{

    static class ReadOnlyCollectionAdapter
    {
        public static ReadOnlyCollectionAdapter<T> Create<T>(ICollection<T> collection) =>
            new ReadOnlyCollectionAdapter<T>(collection);
    }

    /// <summary>
    /// Wraps an instance of <see cref="ICollection{T}"/> as an <see cref="IReadOnlyCollection{T}"/>
    /// </summary>
    class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> m_Collection;


        public int Count => m_Collection.Count;


        public ReadOnlyCollectionAdapter(ICollection<T> collection)
        {
            m_Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }


        public IEnumerator<T> GetEnumerator() => m_Collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_Collection.GetEnumerator();
        
    }
}
