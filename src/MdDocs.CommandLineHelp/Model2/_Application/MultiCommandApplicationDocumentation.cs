using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Encapsulates information about a command line application that provides multiple sub-commands.
    /// </summary>
    public class MultiCommandApplicationDocumentation : ApplicationDocumentation
    {
        private readonly Dictionary<string, CommandDocumentation> m_Commands = new Dictionary<string, CommandDocumentation>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the application's commands
        /// </summary>
        public IEnumerable<CommandDocumentation> Commands => m_Commands.Values.OrderBy(x => x.Name);


        /// <summary>
        /// Initializes a new instance of <see cref="MultiCommandApplicationDocumentation"/>.
        /// </summary>
        public MultiCommandApplicationDocumentation(string name, string? version) : base(name, version)
        { }


        public CommandDocumentation AddCommand(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new InvalidModelException("Command names must not be null or whitespace");

            if (m_Commands.ContainsKey(name))
                throw new InvalidModelException($"Cannot add command '{name}' because a command with the same name already exists");

            var command = new CommandDocumentation(this, name);
            m_Commands.Add(name, command);

            return command;
        }
    }
}
