using System;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    /// <summary>
    /// Encapsulates information for linking to other documentation items.
    /// </summary>
    internal class Link
    {
        /// <summary>
        /// Gets the link's target as absolute path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the anchor of the link target within the target path. Can be <c>null</c>.
        /// </summary>
        public string Anchor { get; }

        /// <summary>
        /// Determines whether the link provides an anchor.
        /// </summary>
        /// <seealso cref="Anchor" />
        public bool HasAnchor => !String.IsNullOrEmpty(Anchor);


        public Link(string path) : this(path, default)
        { }

        public Link(string path, string anchor)
        {
            Path = path;
            Anchor = anchor;
        }
    }
}
