using System;
using System.Collections.Generic;
using System.Text;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    internal class ParameterCollection : IParameterCollection
    {
        private readonly ApplicationDocumentation m_Application;
        private readonly CommandDocumentation? m_Command;
        private readonly List<NamedParameterDocumentation> m_NamedParameters = new List<NamedParameterDocumentation>();
        private readonly List<PositionalParameterDocumentation> m_PositionalParameters = new List<PositionalParameterDocumentation>();
        private readonly List<SwitchParameterDocumentation> m_SwitchParameters = new List<SwitchParameterDocumentation>();

        /// <inheritdoc />
        public IEnumerable<NamedParameterDocumentation> NamedParameters => m_NamedParameters;

        /// <inheritdoc />
        public IEnumerable<PositionalParameterDocumentation> PositionalParameters => m_PositionalParameters;

        /// <inheritdoc />
        public IEnumerable<SwitchParameterDocumentation> SwitchParameters => m_SwitchParameters;


        /// <summary>
        /// Initializes a new instance of <see cref="ParameterCollection"/>
        /// </summary>
        /// <param name="application">The application this parameter collection belongs to.</param>
        /// <param name="command">The command that defines the parameters or <c>null</c> if the parameter is defined by a single-command application</param>
        public ParameterCollection(ApplicationDocumentation application, CommandDocumentation? command)
        {
            m_Application = application ?? throw new ArgumentNullException(nameof(application));
            m_Command = command;
        }


        /// <inheritdoc />
        public NamedParameterDocumentation AddNamedParameter(string? name, string? shortName)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException($"{nameof(name)} and {nameof(shortName)} must not both be empty");

            //TODO: Check if a parameter with that name already exists
            //TODO: Check for conflicts with switch parameters

            var parameter = new NamedParameterDocumentation(m_Application, m_Command, name, shortName);
            m_NamedParameters.Add(parameter);

            return parameter;
        }

        /// <inheritdoc />
        public PositionalParameterDocumentation AddPositionalParameter(int position)
        {
            //TODO: Check if a parameter with that position

            var parameter = new PositionalParameterDocumentation(m_Application, m_Command, position);
            m_PositionalParameters.Add(parameter);

            return parameter;
        }

        /// <inheritdoc />
        public SwitchParameterDocumentation AddSwitchParameter(string? name, string? shortName)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException($"{nameof(name)} and {nameof(shortName)} must not both be empty");

            //TODO: Check if a parameter with that name already exists
            //TODO: Check for conflicts with named parameters

            var parameter = new SwitchParameterDocumentation(m_Application, m_Command, name, shortName);
            m_SwitchParameters.Add(parameter);

            return parameter;
        }
    }
}
