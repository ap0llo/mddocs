namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Base class to text elements in XML documentation comments
    /// </summary>
    public abstract class Element
    {
        // private protected constructor => prevent implementation outside of this assembly
        private protected Element()
        { }

        /// <summary>
        /// Calls the appropriate <c>Visit</c> method for this element on the specified visitor.
        /// </summary>
        public abstract void Accept(IVisitor visitor);

        /// <inheritdoc />
        public abstract override int GetHashCode();

        /// <inheritdoc />
        public abstract override bool Equals(object? obj);
    }
}
