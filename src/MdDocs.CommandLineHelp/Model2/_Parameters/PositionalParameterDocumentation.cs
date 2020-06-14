using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Represents a commandline parameter identified by it's position.
    /// </summary>
    public class PositionalParameterDocumentation : ParameterDocumentation
    {
        /// <summary>
        /// Gets the parameter's position (zero-based)
        /// </summary>
        public int Position { get; }

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
        /// Gets or sets the parameter's "informational" name.
        /// The informational name the name by which the parameter is referred to but has no influence on parsing.
        /// </summary>
        public string? InformationalName { get; set; }

        /// <summary>
        /// Gets or sets the name to use a placeholder for the parameter's name in documentation.
        /// </summary>
        public string? ValuePlaceHolderName { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="PositionalParameterDocumentation"/>
        /// </summary>
        public PositionalParameterDocumentation(ApplicationDocumentation application, CommandDocumentation? command, int position) : base(application, command)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must not be negative");

            Position = position;
        }
    }
}
