using Grynwald.Utilities.Collections;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class ModuleDocumentation : IDocumentation
    {
        private readonly IDictionary<TypeName, TypeDocumentation> m_Types;


        public AssemblyDocumentation AssemblyDocumentation { get; }

        public ModuleDefinition Definition { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }



        public ModuleDocumentation(AssemblyDocumentation assemblyDocumentation, ModuleDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            AssemblyDocumentation = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));


            m_Types = Definition.Types
                .Where(t => t.IsPublic)
                .ToDictionary(typeDefinition => new TypeName(typeDefinition), typeDefinition => new TypeDocumentation(this, typeDefinition));

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);

        }


        public TypeDocumentation TryGetDocumentation(TypeName type) => m_Types.GetValueOrDefault(type);

    }
}
