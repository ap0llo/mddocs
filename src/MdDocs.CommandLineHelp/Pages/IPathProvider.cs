using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public interface IPathProvider
    {
        string GetPath(SingleCommandApplicationDocumentation model);

        string GetPath(MultiCommandApplicationDocumentation model);

        string GetPath(CommandDocumentation model);
    }
}
