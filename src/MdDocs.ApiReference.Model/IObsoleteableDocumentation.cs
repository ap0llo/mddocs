namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Interface for documentation items that can be marked as obsolete.
    /// </summary>
    public interface IObsoleteableDocumentation
    {
        /// <summary>
        /// Gets whether the item is marked as obsolete.
        /// </summary>
        bool IsObsolete { get; }

        /// <summary>
        /// Gets the obsolete message for the item..
        /// </summary>
        /// <value>
        /// Gets the message provided in the obsolete attribute if the item was marked as obsolete or
        /// <c>null</c> is no message was provided or the type was not marked as obsolete.
        /// </value>
        /// <seealso cref="ObsoleteAttribute" />
        string ObsoleteMessage { get; }
    }
}
