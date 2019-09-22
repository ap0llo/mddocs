using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public interface IPathProvider
    {
        string GetPath(ApplicationDocumentation model);

        string GetPath(CommandDocumentation model);
    }
}
