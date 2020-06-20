using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Templates;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.MdDocs.Common.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Templates
{
    /// <summary>
    /// Tests for <see cref="CommandLineHelpTemplateProvider"/>
    /// </summary>
    public class ApiReferenceTemplateProviderTest
    {
        private readonly ILogger m_Logger = NullLogger.Instance;

        [Fact]
        public void GetTemplate_throws_InvalidTemplateConfigurationException_for_unknown_template_name()
        {
            var configuration = new ConfigurationProvider().GetDefaultApiReferenceConfiguration();
            configuration.Template.Name = (ApiReferenceConfiguration.TemplateName)(-1);

            Assert.Throws<InvalidTemplateConfigurationException>(
                () => ApiReferenceTemplateProvider.GetTemplate(m_Logger, configuration)
            );
        }

        [Theory]
        [CombinatorialData]
        public void GetTemplate_accepts_all_template_names(ApiReferenceConfiguration.TemplateName templateName)
        {
            var configuration = new ConfigurationProvider().GetDefaultApiReferenceConfiguration();
            configuration.Template.Name = templateName;

            var template = ApiReferenceTemplateProvider.GetTemplate(m_Logger, configuration);
            Assert.NotNull(template);
        }
    }
}
