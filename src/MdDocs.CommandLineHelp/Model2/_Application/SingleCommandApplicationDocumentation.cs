using System;
using System.Collections.Generic;
using System.Text;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    public class SingleCommandApplicationDocumentation : ApplicationDocumentation, IParameterCollection
    {
        private readonly IParameterCollection m_Parameters;


        /// <summary>
        /// Gets the application's named parameters
        /// </summary>
        public IEnumerable<NamedParameterDocumentation> NamedParameters => m_Parameters.NamedParameters;

        /// <summary>
        /// Gets the application's positional parameters
        /// </summary>
        public IEnumerable<PositionalParameterDocumentation> PositionalParameters => m_Parameters.PositionalParameters;

        /// <summary>
        /// Gets the application's switch-parameters
        /// </summary>
        public IEnumerable<SwitchParameterDocumentation> SwitchParameters => m_Parameters.SwitchParameters;


        /// <summary>
        /// Initializes a new instance <see cref="SingleCommandApplicationDocumentation"/>
        /// </summary>
        public SingleCommandApplicationDocumentation(string name, string? version) : base(name, version)
        {
            m_Parameters = new ParameterCollection(this, null);
        }


        /// <summary>
        /// Adds a new named parameter to the application
        /// </summary>
        public NamedParameterDocumentation AddNamedParameter(string? name, string? shortName) => m_Parameters.AddNamedParameter(name, shortName);

        /// <summary>
        /// Adds a new positional parameter to the application
        /// </summary>
        public PositionalParameterDocumentation AddPositionalParameter(int position) => m_Parameters.AddPositionalParameter(position);

        /// <summary>
        /// Adds a new switch-parameter to the application
        /// </summary>
        public SwitchParameterDocumentation AddSwitchParameter(string? name, string? shortName) => m_Parameters.AddSwitchParameter(name, shortName);
    }
}
