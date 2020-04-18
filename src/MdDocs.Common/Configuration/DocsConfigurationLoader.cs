using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    public static class DocsConfigurationLoader
    {
        public static DocsConfiguration GetConfiguation(string configurationFilePath, object? settingsObject = null)
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();
            using var configurationFileStream = GetFileStreamOrEmpty(configurationFilePath);

            return new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                // Use AddJsonStream() because AddJsonFile() assumes the file name
                // is relative to the ConfigurationBuilder's base directory and does not seem to properly
                // handle absolute paths
                .AddJsonStream(configurationFileStream)
                .AddObject(settingsObject)
                .Load();

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

        private static Stream GetFileStreamOrEmpty(string path)
        {
            return File.Exists(path)
                ? File.Open(path, FileMode.Open, FileAccess.Read)
                : (Stream)new MemoryStream(Encoding.ASCII.GetBytes("{ }"));
        }

        private static DocsConfiguration Load(this IConfigurationBuilder builder)
        {
            var configuration = new DocsConfiguration();
            builder
                .Build()
                .GetSection("mddocs")
                .Bind(configuration);

            return configuration;
        }
    }
}
