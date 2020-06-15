namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a command line parameter identified by name.
    /// </summary>
    /// <seealso cref="IPositionalParameterDocumentation"/>
    public interface INamedParameterDocumentation
    {
        /// <summary>
        /// Gets whether the parameter has a name
        /// </summary>
        /// <remarks>
        /// A parameter can have a name (<see cref="Name"/>), a short name (<see cref="ShortName"/>) or both.
        /// </remarks>
        /// <seealso cref="HasShortName"/>.
        bool HasName { get; }

        /// <summary>
        /// Gets whether the parameter has a short name
        /// </summary>
        /// <remarks>
        /// A parameter can have a name (<see cref="Name"/>), a short name (<see cref="ShortName"/>) or both.
        /// </remarks>
        /// <seealso cref="HasName"/>.
        bool HasShortName { get; }

        /// <summary>
        /// Gets the parameter's name
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Gets the parameter's short name
        /// </summary>
        string? ShortName { get; }
    }
}
