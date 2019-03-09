using System;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal class Link
    {
        public string Path { get; }

        public string Anchor { get; }

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
