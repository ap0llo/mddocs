using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{

    /// <summary>
    /// Encapsulates information about a command in a multi-command application.
    /// </summary>
    /// <seealso cref="MultiCommandApplicationDocumentation"/>.
    public class CommandDocumentation : IParameterCollection
    {
        private readonly ParameterCollection m_Parameters;


        /// <summary>
        /// Gets the application this command belong to.
        /// </summary>
        public MultiCommandApplicationDocumentation Application { get; }

        /// <summary>
        /// Gets the name of the command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the command's description if it has any.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets the command's named parameters
        /// </summary>
        public IEnumerable<NamedParameterDocumentation> NamedParameters => m_Parameters.NamedParameters;

        /// <summary>
        /// Gets the command's positional parameters
        /// </summary>
        public IEnumerable<PositionalParameterDocumentation> PositionalParameters => m_Parameters.PositionalParameters;

        /// <summary>
        /// Gets the command's switch-parameters
        /// </summary>
        public IEnumerable<SwitchParameterDocumentation> SwitchParameters => m_Parameters.SwitchParameters;

        /// <summary>
        /// Gets all the commands's parameters (named, positional and switch-parameters)
        /// </summary>
        public IEnumerable<ParameterDocumentation> AllParameters => m_Parameters.AllParameters;


        /// <summary>
        /// Initializes a new instance of <see cref="CommandDocumentation"/>
        /// </summary>
        public CommandDocumentation(MultiCommandApplicationDocumentation application, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            Application = application ?? throw new ArgumentNullException(nameof(application));
            Name = name;
            m_Parameters = new ParameterCollection(application, this);
        }


        /// <summary>
        /// Adds a new named parameter to this command
        /// </summary>
        public NamedParameterDocumentation AddNamedParameter(string? name, string? shortName) => m_Parameters.AddNamedParameter(name, shortName);

        /// <summary>
        /// Adds a new positional parameter to this command
        /// </summary>
        public PositionalParameterDocumentation AddPositionalParameter(int position) => m_Parameters.AddPositionalParameter(position);

        /// <summary>
        /// Adds a new switch-parameter to this command
        /// </summary>
        public SwitchParameterDocumentation AddSwitchParameter(string? name, string? shortName) => m_Parameters.AddSwitchParameter(name, shortName);

    }
}
