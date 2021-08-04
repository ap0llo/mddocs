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
            var builder = new ApiReferenceBuilder();

            foreach (var assemblyDefinition in assemblyDefinitions)
            {
                _ = builder.AddAssembly(assemblyDefinition.Name.Name, assemblyDefinition.GetInformationalVersionOrVersion());

                foreach (var typeDefinition in assemblyDefinition.MainModule.Types.Where(t => t.IsPublic))
                {
                    LoadTypeRecursively(builder, typeDefinition);
                }
            }

            return builder.Build();
        }


        private void LoadTypeRecursively(ApiReferenceBuilder builder, TypeDefinition typeDefinition)
        {
            var typeId = typeDefinition.ToTypeId();
            _ = builder.AddType(typeDefinition.Module.Assembly.Name.Name, typeId);

            if (typeDefinition.HasNestedTypes)
            {
                foreach (var nestedType in typeDefinition.NestedTypes.Where(x => x.IsNestedPublic))
                {
                    LoadTypeRecursively(builder, nestedType);
                }
            }
        }
    }
}
