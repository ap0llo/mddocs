using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public class ValueDocumentation : ParameterDocumentation
    {
        public int Index { get; }

        public string Name { get; }


        public ValueDocumentation(int index, bool required = false, string name = null, string helpText = null, bool hidden = false, object @default = null, string metaValue = null)
            : base(required: required, helpText: helpText, hidden: hidden, @default: @default, metaValue: metaValue)
        {
            Index = index;
            Name = name;
        }


        private ValueDocumentation(PropertyDefinition property) : base(property.GetAttribute(Constants.ValueAttributeFullName))
        {
            var valueAttribute = property.GetAttribute(Constants.ValueAttributeFullName);

            Index = (int)valueAttribute.ConstructorArguments.Single().Value;
            Name = valueAttribute.GetPropertyValueOrDefault<string>("MetaName");
        }

        public static ValueDocumentation FromPropertyDefinition(PropertyDefinition property, ILogger logger) =>
            new ValueDocumentation(property);
    }
}
