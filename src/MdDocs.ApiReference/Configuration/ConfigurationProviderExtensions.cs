using Grynwald.MdDocs.Common.Configuration;

namespace Grynwald.MdDocs.ApiReference.Configuration
{
    public static class ConfigurationProviderExtensions
    {
        public static ApiReferenceConfiguration GetApiReferenceConfiguration(this ConfigurationProvider configurationLoader)
        {
            return configurationLoader.GetConfiguration<ApiReferenceConfiguration>("apireference");
        }

        public static ApiReferenceConfiguration GetDefaultApiReferenceConfiguration(this ConfigurationProvider configurationLoader)
        {
            return configurationLoader.GetDefaultConfiguration<ApiReferenceConfiguration>("apireference");
        }
    }
}
