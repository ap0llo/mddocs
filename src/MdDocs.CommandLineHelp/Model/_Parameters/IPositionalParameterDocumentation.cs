namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a command line parameter identified by it's position in the commandline arguments.
    /// Positional parameters always have a value, hence this interface is an extension of <see cref="IValuedParameterDocumentation"/>
    /// </summary>
    /// <seealso cref="INamedParameterDocumentation"/>
    public interface IPositionalParameterDocumentation : IValuedParameterDocumentation
    {
        /// <summary>
        /// Gets the parameter's position (zero-based)
        /// </summary>
        int Position { get; }
    }
}
