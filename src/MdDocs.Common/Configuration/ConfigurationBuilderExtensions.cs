using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    internal static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddObject(this IConfigurationBuilder builder, object? settingsObject)
        {
            if (settingsObject is null)
                return builder;

            var settings = GetSettingsDictionary(settingsObject);
            return builder.AddInMemoryCollection(settings);
        }

        internal static Dictionary<string, string> GetSettingsDictionary(object settingsObject)
        {
            var settings = new Dictionary<string, string>();
            foreach (var property in settingsObject.GetType().GetProperties())
            {
                var attribute = property.GetCustomAttribute<ConfigurationValueAttribute>();
                if (attribute is null)
                    continue;

                if (String.IsNullOrEmpty(attribute.Key))
                    continue;

                if (property.PropertyType != typeof(string))
                    throw new InvalidOperationException($"Property '{property.Name}' must be of type string.");

                if (property.GetMethod is null)
                    throw new InvalidOperationException($"Property '{property.Name}' does not have a getter");

                var value = (string?)property.GetMethod.Invoke(settingsObject, Array.Empty<object>());

                if (value is string)
                    settings.Add(attribute.Key, value);
            }

            return settings;
        }
    }
}
