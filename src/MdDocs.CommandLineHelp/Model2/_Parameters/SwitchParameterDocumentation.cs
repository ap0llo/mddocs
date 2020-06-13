using System;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Represents a "switch" commandline parameter (parameter without a value)
    /// </summary>
    public class SwitchParameterDocumentation : ParameterDocumentation
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
