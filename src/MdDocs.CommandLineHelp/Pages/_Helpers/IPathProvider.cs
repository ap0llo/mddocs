using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    //TODO: Path provider should accept page instances, not model instances. Mapping Model-> Page should be responsibility of Page factory
    public interface IPathProvider
    {
        string GetPath(ApplicationDocumentation model);

        string GetPath(CommandDocumentation model);
    }
}
