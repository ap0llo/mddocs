using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents an positional parameter.
    /// </summary>
    public class ValueDocumentation : ParameterDocumentation
    {
        public int Index { get; }

        // Values might have a name, but name is not used to bind the parameter when parsing.
        public override string? Name { get; }

        public override bool HasName => !String.IsNullOrEmpty(Name);


        internal ValueDocumentation(
            int index, bool required = false, string? name = null, string? helpText = null, bool hidden = false,
            object? @default = null, string? metaValue = null, IReadOnlyList<string>? acceptedValues = null)
            : base(required: required, helpText: helpText, hidden: hidden, @default: @default, metaValue: metaValue, acceptedValues: acceptedValues)
        {
            Index = index;
            Name = name;
        }

        private ValueDocumentation(PropertyDefinition property) : base(property, property.GetAttribute(CommandLineParserTypeNames.ValueAttributeFullName))
        {
            var valueAttribute = property.GetAttribute(CommandLineParserTypeNames.ValueAttributeFullName);

            Index = (int)valueAttribute.ConstructorArguments.Single().Value;
            Name = valueAttribute.GetPropertyValueOrDefault<string>("MetaName");
        }


        public static ValueDocumentation FromPropertyDefinition(PropertyDefinition property, ILogger logger) => new ValueDocumentation(property);
    }
}
