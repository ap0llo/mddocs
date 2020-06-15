using Grynwald.MdDocs.CommandLineHelp.Model;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Loaders
{
    internal interface IDocumentationLoader
    {
        ApplicationDocumentation Load(AssemblyDefinition assembly);
    }
}
