using System;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Represents a "switch" commandline parameter (parameter without a value)
    /// </summary>
    public class SwitchParameterDocumentation : ParameterDocumentation, INamedParameterDocumentation
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
