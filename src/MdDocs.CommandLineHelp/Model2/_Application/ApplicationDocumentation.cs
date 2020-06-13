using System;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    /// <summary>
    /// Represents documentation about any command line application.
    /// </summary>
    public abstract class ApplicationDocumentation
    {
        //TODO: Usage

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        public string? Version { get; }


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
