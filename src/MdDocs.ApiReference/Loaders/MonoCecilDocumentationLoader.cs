using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    //TODO 2021-08-04: Logging
    public class MonoCecilDocumentationLoader : IDocumentationLoader
    {
        public _AssemblySetDocumentation Load(IEnumerable<AssemblyDefinition> assemblyDefinitions)
        {
            var builder = new AssemblySetDocumentationBuilder();

            foreach (var assemblyDefinition in assemblyDefinitions)
            {
                _ = builder.AddAssembly(assemblyDefinition.Name.Name, assemblyDefinition.GetInformationalVersionOrVersion());

                foreach (var typeDefinition in assemblyDefinition.MainModule.Types.Where(t => t.IsPublic))
                {
                    LoadTypeRecursively(builder, typeDefinition, declaringType: null);
                }
            }

            return builder.Build();
        }


        private void LoadTypeRecursively(AssemblySetDocumentationBuilder builder, TypeDefinition typeDefinition, TypeDocumentation? declaringType)
        {
            var typeId = typeDefinition.ToTypeId();
            var type = builder.AddType(typeDefinition.Module.Assembly.Name.Name, typeId);

            // TODO 2021-08-04: Load nested types
        }
    }
}
