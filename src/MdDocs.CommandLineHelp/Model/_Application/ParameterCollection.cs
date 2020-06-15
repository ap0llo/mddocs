using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    internal class ParameterCollection : IParameterCollection
    {
        private readonly ApplicationDocumentation m_Application;
        private readonly CommandDocumentation? m_Command;
        private readonly List<NamedParameterDocumentation> m_NamedParameters = new List<NamedParameterDocumentation>();
        private readonly IDictionary<int, PositionalParameterDocumentation> m_PositionalParameters = new Dictionary<int, PositionalParameterDocumentation>();
        private readonly List<SwitchParameterDocumentation> m_SwitchParameters = new List<SwitchParameterDocumentation>();
        private readonly HashSet<string> m_ParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> m_ParameterShortNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);


        /// <inheritdoc />
        public IEnumerable<ParameterDocumentation> AllParameters =>
            PositionalParameters.Concat(NamedParameters.Concat(SwitchParameters.Cast<INamedParameterDocumentation>()).OrderBy(x => x.Name).Cast<ParameterDocumentation>());

        /// <inheritdoc />
        public IEnumerable<NamedParameterDocumentation> NamedParameters =>
            Enumerable.Concat(
                m_NamedParameters.Where(p => p.HasName).OrderBy(x => x.Name),
                m_NamedParameters.Where(x => !x.HasName).OrderBy(x => x.ShortName));


        /// <inheritdoc />
        public IEnumerable<PositionalParameterDocumentation> PositionalParameters => m_PositionalParameters.Values.OrderBy(x => x.Position);

        /// <inheritdoc />
        public IEnumerable<SwitchParameterDocumentation> SwitchParameters =>
            Enumerable.Concat(
                m_SwitchParameters.Where(x => x.HasName).OrderBy(x => x.Name),
                m_SwitchParameters.Where(x => !x.HasName).OrderBy(x => x.ShortName));


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
            AssertCanAddParameter(name, shortName);

            AddParameterName(name, shortName);

            var parameter = new NamedParameterDocumentation(m_Application, m_Command, name, shortName);
            m_NamedParameters.Add(parameter);

            return parameter;
        }

        /// <inheritdoc />
        public PositionalParameterDocumentation AddPositionalParameter(int position)
        {
            AssertCanAddParameter(position);

            var parameter = new PositionalParameterDocumentation(m_Application, m_Command, position);
            m_PositionalParameters.Add(position, parameter);

            return parameter;
        }

        /// <inheritdoc />
        public SwitchParameterDocumentation AddSwitchParameter(string? name, string? shortName)
        {
            AssertCanAddParameter(name, shortName);

            AddParameterName(name, shortName);

            var parameter = new SwitchParameterDocumentation(m_Application, m_Command, name, shortName);
            m_SwitchParameters.Add(parameter);

            return parameter;
        }



        private void AssertCanAddParameter(string? name, string? shortName)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(shortName))
                throw new InvalidModelException("A parameter's name and short name must not both be empty.");


            if (!String.IsNullOrWhiteSpace(name) && m_ParameterNames.Contains(name))
            {
                throw new InvalidModelException($"Cannot add named parameter because a parameter named '{name}' already exists.");
            }

            if (!String.IsNullOrWhiteSpace(shortName) && m_ParameterShortNames.Contains(shortName))
            {
                throw new InvalidModelException($"Cannot add named parameter because a parameter with a short name of '{shortName}' already exists.");
            }
        }

        private void AssertCanAddParameter(int position)
        {
            if (position < 0)
                throw new InvalidModelException("A positional parameter's position must not be negative.");

            if (m_PositionalParameters.ContainsKey(position))
                throw new InvalidModelException($"Cannot add positional parameter because a parameter at position {position} already exists.");
        }

        private void AddParameterName(string? name, string? shortName)
        {
            if (!String.IsNullOrWhiteSpace(name))
                m_ParameterNames.Add(name);

            if (!String.IsNullOrWhiteSpace(shortName))
                m_ParameterShortNames.Add(shortName);
        }

    }
}
