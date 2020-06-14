using System;
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

                if (property.PropertyType == typeof(string) && property.GetCustomAttribute<ConvertToFullPathAttribute>() != null)
                {
                    // convert the value of string properties with a ConvertToFullPath attribute to absolute paths
                    var value = (string)propertyValue;
                    var absolutePath = GetFullPath(value, baseDirectory);
                    property.SetValue(configurationObject, absolutePath);
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
