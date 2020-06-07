namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base interface for all documentation model types.
    /// </summary>
    public interface IDocumentation
    {
        /// <summary>
        /// Tries to get the documentation for the specified member
        /// </summary>
        /// <returns>Returns documentation for the specified member or null if it is unknown or not documented</returns>
        IDocumentation? TryGetDocumentation(MemberId member);

        /// <summary>
        /// Gets the documentation object for the assembly
        /// </summary>
        AssemblyDocumentation GetAssemblyDocumentation();
    }
}
