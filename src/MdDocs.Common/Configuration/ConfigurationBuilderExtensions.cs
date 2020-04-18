using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    public static class ConfigurationBuilderExtensions
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

                if (!IsSupportedPropertyType(property.PropertyType))
                    throw new InvalidOperationException($"Property type {property.PropertyType} of property '{property.Name}' is not supported");

                if (property.GetMethod is null)
                    throw new InvalidOperationException($"Property '{property.Name}' does not have a getter");

                var value = property.GetMethod.Invoke(settingsObject, Array.Empty<object>());

                if (value is object)
                    settings.Add(attribute.Key, Convert.ToString(value));
            }

            return settings;
        }

        internal static bool IsSupportedPropertyType(Type propertyType)
        {
            if (propertyType == typeof(string))
                return true;

            if (propertyType == typeof(bool))
                return true;

            return false;
        }
    }
}
