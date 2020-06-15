using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a commandline parameter identified by it's position.
    /// </summary>
    [DebuggerDisplay("PositionalParameter ({Position})")]
    public class PositionalParameterDocumentation : ParameterDocumentation, IPositionalParameterDocumentation
    {
        /// <inheritdoc />
        public int Position { get; }

        /// <inheritdoc />
        public bool Required { get; set; } = false;

        /// <inheritdoc />
        public string? DefaultValue { get; set; } = null;

        /// <inheritdoc />
        public IReadOnlyList<string>? AcceptedValues { get; set; } = null;

        /// <inheritdoc />
        public string? InformationalName { get; set; }

        /// <inheritdoc />
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
