using System.Collections.Generic;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Test
{
    internal static class ParameterCollectionExtensions
    {
        public static T WithNamedParameter<T>(
            this T command,
            string? name = null,
            string? shortName = null,
            bool? required = null,
            string? description = null,
            string? defaultValue = null,
            IReadOnlyList<string>? acceptedValues = null,
            string? valuePlaceHolderName = null) where T : IParameterCollection
        {
            var parameter = command.AddNamedParameter(name, shortName);

            if (required.HasValue)
            {
                parameter.Required = required.Value;
            }

            parameter.Description = description;
            parameter.DefaultValue = defaultValue;
            parameter.AcceptedValues = acceptedValues;
            parameter.ValuePlaceHolderName = valuePlaceHolderName;

            return command;
        }

        public static T WithPositionalParameter<T>(
            this T command,
            int position,
            bool? required = null,
            string? description = null,
            string? defaultValue = null,
            IReadOnlyList<string>? acceptedValues = null,
            string? informationalName = null,
            string? valuePlaceHolderName = null) where T : IParameterCollection
        {
            var parameter = command.AddPositionalParameter(position);

            if (required.HasValue)
            {
                parameter.Required = required.Value;
            }

            parameter.Description = description;
            parameter.DefaultValue = defaultValue;
            parameter.AcceptedValues = acceptedValues;
            parameter.InformationalName = informationalName;
            parameter.ValuePlaceHolderName = valuePlaceHolderName;

            return command;
        }

        public static T WithSwitchParameter<T>(
            this T command,
            string? name = null,
            string? shortName = null,
            string? description = null) where T : IParameterCollection
        {
            var parameter = command.AddSwitchParameter(name, shortName);

            parameter.Description = description;

            return command;
        }

    }
}
