using System;
using System.Collections.Generic;
using System.Text;
using Grynwald.MdDocs.CommandLineHelp.Model2;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Loaders
{
    interface IDocumentationLoader
    {
        ApplicationDocumentation Load(AssemblyDefinition assembly);
    }
}
