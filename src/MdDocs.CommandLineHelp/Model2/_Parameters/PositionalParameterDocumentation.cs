using System;

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


        //TODO: Accepted values

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
