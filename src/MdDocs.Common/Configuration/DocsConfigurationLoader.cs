using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Grynwald.Utilities.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    public static class DocsConfigurationLoader
    {
        private const string s_RootSectionName = "mddocs";


        public static DocsConfiguration GetConfiguration(string configurationFilePath, object? settingsObject = null)
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();
            using var configurationFileStream = GetFileStreamOrEmpty(configurationFilePath, out var configFileLoaded);

            var config = new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                // Use AddJsonStream() because AddJsonFile() assumes the file name
                // is relative to the ConfigurationBuilder's base directory and does not seem to properly
                // handle absolute paths
                .AddJsonStream(configurationFileStream)
                .AddObject(settingsObject)
                .Load();

            if (configFileLoaded)
            {
                // convert relative paths in the configuration file to absolute paths.
                var baseDirectory = Path.GetDirectoryName(Path.GetFullPath(configurationFilePath));

                config.ApiReference.OutputPath = GetFullPath(config.ApiReference.OutputPath, baseDirectory);
                config.CommandLineHelp.OutputPath = GetFullPath(config.CommandLineHelp.OutputPath, baseDirectory);
            }

            return config;
        }

        public static DocsConfiguration GetDefaultConfiguration()
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();

            return new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                .Load();
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

        private static DocsConfiguration Load(this IConfigurationBuilder builder)
        {
            var configuration = new DocsConfiguration();
            builder
                .Build()
                .GetSection(s_RootSectionName)
                .Bind(configuration);

            return configuration;
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
    }
}
