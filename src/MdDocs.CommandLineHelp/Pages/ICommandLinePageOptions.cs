namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public interface ICommandLinePageOptions
    {
        /// <summary>
        /// Gets the setting whether to include the application version in the generated documentation
        /// </summary>
        bool IncludeVersion { get; }
    }
}
