using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents documentation about any command line application.
    /// </summary>
    public abstract class ApplicationDocumentation
    {
        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// Gets or sets usage information/examples for the application.
        /// </summary>
        public IReadOnlyList<string>? Usage { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationDocumentation"/>.
        /// </summary>
        protected ApplicationDocumentation(string name, string? version)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            Name = name;
            Version = version;
        }
    }
}
