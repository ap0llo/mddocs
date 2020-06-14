using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Represents a command line parameter identified by name.
    /// </summary>
    public class NamedParameterDocumentation : ParameterDocumentation
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
