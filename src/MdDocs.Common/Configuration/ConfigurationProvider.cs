using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Grynwald.Utilities.Configuration;
using Microsoft.Extensions.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    public class ConfigurationProvider
    {
        private const string s_RootSectionName = "mddocs";

        private readonly string m_ConfigurationFilePath;
        private readonly object? m_SettingsObject;

        public ConfigurationProvider() : this("", null)
        { }

        public ConfigurationProvider(string configurationFilePath, object? settingsObject = null)
        {
            m_ConfigurationFilePath = configurationFilePath;
            m_SettingsObject = settingsObject;
        }


        public T GetConfiguration<T>(string sectionName) where T : notnull, new()
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();
            using var configurationFileStream = GetFileStreamOrEmpty(m_ConfigurationFilePath, out var configFileLoaded);

            var config = new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                // Use AddJsonStream() because AddJsonFile() assumes the file name
                // is relative to the ConfigurationBuilder's base directory and does not seem to properly
                // handle absolute paths
                .AddJsonStream(configurationFileStream)
                .AddObject(m_SettingsObject)
                .Load<T>(GetFullSectionName(sectionName));



            // By default, ConfigurationBuilder combines lists from different configuration sources.
            // For example, assuming the is a string[] property named "MyArray" in T
            // If the JSON file is { "MyArray" : [ "a", "b", "c"] } and the settings object defines a value
            // for the "MyArray" setting of ["d", "e"], the values for the settings object would just overwrite
            // the fist two elements of the array and the combined configuration value would be ["d", "e", "c" ]
            //
            // This is not the desired behavior here.
            // Instead, if the settings object defines and value for "MyArray", this value should replace
            // the value for the array from the JSON file - thus resulting in an configuration value of ["d", "e"].
            // (for example, it must be possible override the assembly paths configured in the JSON configuration file using command line parameters)
            //
            // To achieve this, a configuration object is constructed from *just* the settings object.
            // For all properties in T where the configuration constructed from just the settings object has a non-empty array,
            // replace the value in the configuration with the value from the settings object.

            var settingsObjectConfigOnly = new ConfigurationBuilder()
                .AddObject(m_SettingsObject)
                .Load<T>(GetFullSectionName(sectionName));

            foreach (var property in config.GetType().GetProperties().Where(x => x.PropertyType.IsArray))
            {
                if (property.GetValue(settingsObjectConfigOnly) is IEnumerable<object> enumerableValue && enumerableValue.Any())
                {
                    property.SetValue(config, enumerableValue);
                }
            }

            if (configFileLoaded)
            {
                var baseDirectory = Path.GetDirectoryName(Path.GetFullPath(m_ConfigurationFilePath))!;
                ResolveRelativePaths(config, baseDirectory);
            }

            return config;
        }

        public T GetDefaultConfiguration<T>(string sectionName) where T : new()
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();

            return new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                .Load<T>(GetFullSectionName(sectionName));
        }


        private static Stream? GetDefaultSettingsStream() =>
            Assembly.GetExecutingAssembly().GetManifestResourceStream("Grynwald.MdDocs.Common.Configuration.defaultSettings.json");

        private static Stream GetFileStreamOrEmpty(string path, out bool fileLoaded)
        {
            if (!String.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                fileLoaded = true;
                return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                fileLoaded = false;
                return new MemoryStream(Encoding.ASCII.GetBytes("{ }"));
            }
        }


        private static string GetFullPath(string path, string baseDirectory)
        {
            if (String.IsNullOrEmpty(path))
                return path;

            if (!Path.IsPathRooted(path))
                path = Path.Combine(baseDirectory, path);

            path = Path.GetFullPath(path);
            return path;
        }

        private static void ResolveRelativePaths(object configurationObject, string baseDirectory)
        {
            foreach (var property in configurationObject.GetType().GetProperties().Where(x => x.GetIndexParameters().Length == 0))
            {
                if (property == null)
                    throw new InvalidOperationException();

                var propertyValue = property.GetValue(configurationObject);

                if (propertyValue is null)
                    continue;

                // convert the value of string properties with a ConvertToFullPath attribute to absolute paths
                if (property.PropertyType is not null && property.GetCustomAttribute<ConvertToFullPathAttribute>() is not null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var value = (string)propertyValue;
                        var absolutePath = GetFullPath(value, baseDirectory);
                        property.SetValue(configurationObject, absolutePath);
                    }
                    else if (property.PropertyType.IsArray && property.PropertyType.GetArrayRank() == 1 && property.PropertyType.GetElementType() == typeof(string))
                    {
                        var value = (string[])propertyValue;
                        var absolutePaths = value.Select(x => GetFullPath(x, baseDirectory)).ToArray();
                        property.SetValue(configurationObject, absolutePaths);
                    }
                }
                else if (property.PropertyType?.Namespace?.StartsWith("System.") == true || property.PropertyType?.Namespace == "System")
                {
                    // ignore properties of "System" types
                    continue;
                }
                else
                {
                    // recursively check all properties for ConvertToFullPath attributes                    
                    ResolveRelativePaths(propertyValue, baseDirectory);
                }
            }
        }

        private string GetFullSectionName(string sectionName)
        {
            return String.IsNullOrEmpty(sectionName)
                ? s_RootSectionName
                : $"{s_RootSectionName}:{sectionName}";
        }

    }
}
