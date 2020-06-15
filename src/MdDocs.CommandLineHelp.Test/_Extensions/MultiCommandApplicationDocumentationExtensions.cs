using Grynwald.MdDocs.CommandLineHelp.Model2;

namespace Grynwald.MdDocs.CommandLineHelp.Test
{
    internal static class MultiCommandApplicationDocumentationExtensions
    {
        public static MultiCommandApplicationDocumentation WithCommand(this MultiCommandApplicationDocumentation application, string name, string? helpText = null)
        {
            var command = application.AddCommand(name);

            command.Description = helpText;

            return application;
        }

    }
}
