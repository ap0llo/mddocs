namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a command line parameter.
    /// </summary>
    public abstract class ParameterDocumentation
    {
        /// <summary>
        /// Gets the application the command belong to
        /// </summary>
        public ApplicationDocumentation Application { get; }

        /// <summary>
        /// Gets the command the parameter belong to.
        /// </summary>
        /// <value>
        /// The command that defines the parameter or <c>null</c> if the parameter is defined by a single-command application
        /// </value>
        public CommandDocumentation? Command { get; }

        /// <summary>
        /// Gets the parameters's description if it has any.
        /// </summary>
        public string? Description { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="ParameterDocumentation"/>
        /// </summary>
        protected ParameterDocumentation(ApplicationDocumentation application, CommandDocumentation? command)
        {
            Application = application ?? throw new System.ArgumentNullException(nameof(application));
            Command = command;
        }
    }
}
