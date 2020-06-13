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
        private readonly List<CommandDocumentation> m_Commands = new List<CommandDocumentation>();

        /// <summary>
        /// Gets the application's commands
        /// </summary>
        public IEnumerable<CommandDocumentation> Commands => m_Commands.OrderBy(x => x.Name);


        /// <summary>
        /// Initializes a new instance of <see cref="MultiCommandApplicationDocumentation"/>.
        /// </summary>
        public MultiCommandApplicationDocumentation(string name, string? version) : base(name, version)
        { }


        public CommandDocumentation AddCommand(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            //TODO: Check if a command with the specified name already exists

            var command = new CommandDocumentation(this, name);
            m_Commands.Add(command);

            return command;

        }
    }
}
