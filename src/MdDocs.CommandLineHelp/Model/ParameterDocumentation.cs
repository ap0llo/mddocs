using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    //TODO: Min, Max
    public abstract class ParameterDocumentation
    {
        public bool Required { get; }

        public string HelpText { get; }

        public bool Hidden { get; }

        public object Default { get; }

        public string MetaValue { get; }

        public IReadOnlyList<string> AcceptedValues { get; } 

        public bool HasDefault => Default != null;

        public bool HasMetaValue => !String.IsNullOrEmpty(MetaValue);

        public bool HasAcceptedValues => AcceptedValues != null;

        public abstract string Name { get; }

        public abstract bool HasName  { get; }

        public abstract bool HasShortName { get; }

        

        public ParameterDocumentation(bool required, string helpText, bool hidden, object @default, string metaValue, IReadOnlyList<string> acceptedValues)
        {
            Required = required;
            HelpText = helpText;
            Hidden = hidden;
            Default = @default;
            MetaValue = metaValue;
            AcceptedValues = acceptedValues;
        }


        protected ParameterDocumentation(PropertyDefinition property, CustomAttribute attribute)
        {
            Required = attribute.GetPropertyValueOrDefault<bool>("Required");
            Hidden = attribute.GetPropertyValueOrDefault<bool>("Hidden");
            Default = attribute.GetPropertyValueOrDefault<object>("Default");
            HelpText = attribute.GetPropertyValueOrDefault<string>("HelpText");
            MetaValue = attribute.GetPropertyValueOrDefault<string>("MetaValue");
            AcceptedValues = GetAcceptedValues(property);
        }



        private IReadOnlyList<string> GetAcceptedValues(PropertyDefinition property)
        {
            var type = property.PropertyType.Resolve();

            if (type == null || !type.IsEnum)
                return null;

            return type.Fields
                .Where(f => f.IsPublic && !f.IsSpecialName)
                .Select(f => f.Name)
                .ToArray();
        }
    }
}
