using Microsoft.Extensions.Configuration;

namespace Grynwald.MdDocs.Common.Configuration
{
    internal static class ConfigurationBuilderExtensions
    {
        public static T Load<T>(this IConfigurationBuilder builder, string sectionName) where T : new()
        {
            var configuration = new T();
            builder
                .Build()
                .GetSection(sectionName)
                .Bind(configuration);

            return configuration;
        }
    }
}
