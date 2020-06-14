using System.Collections.Generic;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    public interface IParameterCollection
    {
        /// <summary>
        /// Gets all the collections's parameters (named, positional and switch-parameters)
        /// </summary>
        IEnumerable<ParameterDocumentation> AllParameters { get; }

        /// <summary>
        /// Gets the collection's named parameters
        /// </summary>
        IEnumerable<NamedParameterDocumentation> NamedParameters { get; }

        /// <summary>
        /// Gets the collection's positional parameters
        /// </summary>
        IEnumerable<PositionalParameterDocumentation> PositionalParameters { get; }

        /// <summary>
        /// Gets the collection's switch-parameters
        /// </summary>
        IEnumerable<SwitchParameterDocumentation> SwitchParameters { get; }


        /// <summary>
        /// Adds a new named parameter to the collection
        /// </summary>
        NamedParameterDocumentation AddNamedParameter(string? name, string? shortName);

        /// <summary>
        /// Adds a new positional parameter to the collection
        /// </summary>
        PositionalParameterDocumentation AddPositionalParameter(int position);

        /// <summary>
        /// Adds a new switch-parameter to the collection.
        /// </summary>
        SwitchParameterDocumentation AddSwitchParameter(string? name, string? shortName);
    }
}
