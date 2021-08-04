using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    internal interface IDocumentationLoader
    {
        // TODO 2021-08-04: Make interface independent of Mono.Cecil type AssemblyDefinition
        _AssemblySetDocumentation Load(IEnumerable<AssemblyDefinition> assemblyDefinitions);
    }
}
