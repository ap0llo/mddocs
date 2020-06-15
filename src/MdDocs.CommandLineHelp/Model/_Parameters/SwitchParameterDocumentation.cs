using System;
using System.Diagnostics;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a "switch" commandline parameter 
    /// </summary>
    /// <remarks>
    /// Switch parameters represent commandline "switched" and do not accept a value (their are either present or not)
    /// </remarks>
    [DebuggerDisplay("SwitchParameter ({Name}|{ShortName})")]
    public class SwitchParameterDocumentation : ParameterDocumentation, INamedParameterDocumentation
    {
        /// <inheritdoc />
        public string? Name { get; }

        /// <inheritdoc />
        public string? ShortName { get; }

        /// <inheritdoc />
        public bool HasName => !String.IsNullOrWhiteSpace(Name);

        /// <inheritdoc />
        public bool HasShortName => !String.IsNullOrWhiteSpace(ShortName);


        /// <summary>
        /// Initializes a new instance of <see cref="SwitchParameterDocumentation" />
        /// </summary>
        public SwitchParameterDocumentation(ApplicationDocumentation application, CommandDocumentation? command, string? name, string? shortName) : base(application, command)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException($"{nameof(name)} and {nameof(shortName)} must not both be empty");

            Name = name;
            ShortName = shortName;
        }
    }
}
