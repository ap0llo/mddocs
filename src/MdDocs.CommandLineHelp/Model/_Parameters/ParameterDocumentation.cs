using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Base class for command line parameters (both unnamed and positional parameters)
    /// </summary>
    //TODO: Min, Max
    public abstract class ParameterDocumentation
    {
        /// <summary>
        /// Gets whether the parameter is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets the parameter's help text.
        /// </summary>
        /// <value>The help text or <c>null</c> is no help text was specified.</value>
        public string HelpText { get; }

        /// <summary>
        /// Gets whether the parameter is hidden
        /// </summary>
        public bool Hidden { get; }

        /// <summary>
        /// Gets the parameter's default value
        /// </summary>
        /// <value>The parameter's default value or <c>null</c> is no default parameter was specified.</value>
        public object Default { get; }

        /// <summary>
        /// Gets the parameter's meta value (a description of the parameter value)
        /// </summary>
        /// <value>The parameter's meta value or <c>null</c>.</value>
        public string MetaValue { get; }

        /// <summary>
        /// Gets the parameter's possible values.
        /// </summary>
        /// <value>The possible values for the parameter of <c>null</c> is the accepted values are unknown.</value>
        public IReadOnlyList<string> AcceptedValues { get; }

        /// <summary>
        /// Get whether the parameter has a default value.
        /// </summary>
        public bool HasDefault => Default != null;

        /// <summary>
        /// Gets whether the parameter has a meta value
        /// </summary>
        public bool HasMetaValue => !String.IsNullOrEmpty(MetaValue);

        /// <summary>
        /// Gets whether the parameter has a list of accepted values
        /// </summary>
        public bool HasAcceptedValues => AcceptedValues != null;

        /// <summary>
        /// Gets the parameter's name
        /// </summary>
        /// <value>The parameter's name or <c>null</c> if the parameter has no name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets whether the parameter has a name.
        /// </summary>
        public abstract bool HasName { get; }


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
            Default = LoadDefaultValue(property, attribute);
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

        private object LoadDefaultValue(PropertyDefinition property, CustomAttribute attribute)
        {
            var value = attribute.GetPropertyValueOrDefault<object>("Default");

            var type = property.PropertyType.Resolve();
            if (value != null && type.IsEnum)
            {
                var enumValues = type.GetEnumValues();
                var longValue = Convert.ToInt64(value);

                return enumValues.FirstOrDefault(x => x.value == longValue).name;
            }

            return value;
        }
    }
}
