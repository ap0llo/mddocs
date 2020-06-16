using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a command line parameter identified by name that has a value (in contrast to a switch-parameter)
    /// </summary>
    /// <seealso cref="SwitchParameterDocumentation"/>
    [DebuggerDisplay("NamedParameter ({Name}|{ShortName})")]
    public class NamedValuedParameterDocumentation : ParameterDocumentation, INamedParameterDocumentation, IValuedParameterDocumentation
    {
        /// <inheritdoc />
        public string? Name { get; }

        /// <inheritdoc />
        public string? ShortName { get; }

        /// <inheritdoc />
        public bool HasName => !String.IsNullOrWhiteSpace(Name);

        /// <inheritdoc />
        public bool HasShortName => !String.IsNullOrWhiteSpace(ShortName);

        /// <inheritdoc />
        public bool Required { get; set; } = false;

        /// <inheritdoc />
        public string? DefaultValue { get; set; } = null;

        /// <inheritdoc />
        public IReadOnlyList<string>? AcceptedValues { get; set; } = null;

        /// <inheritdoc />
        public string? ValuePlaceHolderName { get; set; }



        /// <summary>
        /// Initializes a new instance of <see cref="NamedValuedParameterDocumentation"/>.
        /// </summary>
        public NamedValuedParameterDocumentation(ApplicationDocumentation application, CommandDocumentation? command, string? name, string? shortName) : base(application, command)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException($"{nameof(name)} and {nameof(shortName)} must not both be empty");

            Name = name;
            ShortName = shortName;
        }
    }
}
