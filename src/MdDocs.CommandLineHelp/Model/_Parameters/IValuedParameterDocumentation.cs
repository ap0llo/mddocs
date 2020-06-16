using System.Collections.Generic;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a parameter that has a value (in contrast to a "switch-parameter").
    /// </summary>
    /// <remarks>
    /// Valued parameters can be both named or positional
    /// </remarks>
    /// <seealso cref="INamedParameterDocumentation"/>
    /// <seealso cref="PositionalParameterDocumentation"/>
    public interface IValuedParameterDocumentation
    {
        /// <summary>
        /// Gets or sets whether the parameter is mandatory
        /// </summary>
        bool Required { get; set; }

        /// <summary>
        /// Gets of sets the parameter's default value.
        /// </summary>
        /// <value>
        /// The string-representation of the default value or <c>null</c> if the parameter does not have a default value.
        /// </value>
        string? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the parameter's allowed values.
        /// </summary>
        /// <value>The possible values for the parameter of <c>null</c> is the accepted values are unknown.</value>
        IReadOnlyList<string>? AcceptedValues { get; set; }

        /// <summary>
        /// Gets or sets the name to use a placeholder for the parameter's name in documentation.
        /// </summary>
        string? ValuePlaceHolderName { get; set; }
    }
}
