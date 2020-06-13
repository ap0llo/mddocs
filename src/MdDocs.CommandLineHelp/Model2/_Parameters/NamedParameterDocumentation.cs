using System;
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


        //TODO: Default
        //TODO: Required
        //TODO: HelpText
        //TODO: Accepted values

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
