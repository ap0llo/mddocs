using System;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class CommandDocumentation
    {
        private readonly TypeDefinition m_Definition;


        public string Name { get; }

        public string HelpText { get; }


        public CommandDocumentation(TypeDefinition definition)
        {
            m_Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            var verbAttribute = definition.CustomAttributes.SingleOrDefault(x => x.AttributeType.FullName == Constants.VerbAttributeFullName);

            Name = (string)verbAttribute.ConstructorArguments.Single().Value;
            HelpText = (string)verbAttribute.Properties.SingleOrDefault(x => x.Name == "HelpText").Argument.Value;
        }
    }
}
