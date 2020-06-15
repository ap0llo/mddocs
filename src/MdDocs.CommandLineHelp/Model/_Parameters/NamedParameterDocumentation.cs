using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a command line parameter identified by name.
    /// </summary>
    [DebuggerDisplay("NamedParameter ({Name}|{ShortName})")]
    public class NamedParameterDocumentation : ParameterDocumentation, INamedParameterDocumentation
    {
        /// <summary>
        /// Gets the parameter's name
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the parameter's short name
        /// </summary>
        public string? ShortName { get; }

        /// <summary>
        /// Gets or sets whether the parameter is mandatory
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Gets of sets the parameter's default value.
        /// </summary>
        /// <value>
        /// The string-representation of the default value or <c>null</c> if the parameter does not have a default value.
        /// </value>
        public string? DefaultValue { get; set; } = null;

        /// <summary>
        /// Gets or sets the parameter's allowed values.
        /// </summary>
        /// <value>The possible values for the parameter of <c>null</c> is the accepted values are unknown.</value>
        public IReadOnlyList<string>? AcceptedValues { get; set; } = null;

        /// <summary>
        /// Gets whether the parameter has a name
        /// </summary>
        /// <remarks>
        /// A parameter can have a name (<see cref="Name"/>), a short name (<see cref="ShortName"/>) or both.
        /// </remarks>
        /// <seealso cref="HasShortName"/>.
        public bool HasName => !String.IsNullOrWhiteSpace(Name);

        /// <summary>
        /// Gets whether the parameter has a short name
        /// </summary>
        /// <remarks>
        /// A parameter can have a name (<see cref="Name"/>), a short name (<see cref="ShortName"/>) or both.
        /// </remarks>
        /// <seealso cref="HasShortName"/>.
        public bool HasShortName => !String.IsNullOrWhiteSpace(ShortName);

        /// <summary>
        /// Gets or sets the name to use a placeholder for the parameter's name in documentation.
        /// </summary>
        public string? ValuePlaceHolderName { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="NamedParameterDocumentation"/>.
        /// </summary>
        public NamedParameterDocumentation(ApplicationDocumentation application, CommandDocumentation? command, string? name, string? shortName) : base(application, command)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException($"{nameof(name)} and {nameof(shortName)} must not both be empty");

            Name = name;
            ShortName = shortName;
        }
    }
}
