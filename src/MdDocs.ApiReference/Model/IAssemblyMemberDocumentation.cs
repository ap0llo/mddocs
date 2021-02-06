namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Interface for all documentation model types that represent an item defined in an assembly (e.g. types, methods)
    /// </summary>
    public interface IAssemblyMemberDocumentation : IDocumentation
    {
        /// <summary>
        /// Gets the documentation object for the assembly the current item is defined in.
        /// </summary>
        AssemblyDocumentation AssemblyDocumentation { get; }
    }
}
