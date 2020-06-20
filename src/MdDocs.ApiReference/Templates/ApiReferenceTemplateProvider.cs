using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Grynwald.MdDocs.Common.Templates;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Templates
{
    public static class ApiReferenceTemplateProvider
    {
        public static ITemplate<AssemblyDocumentation> GetTemplate(ILogger logger, ApiReferenceConfiguration configuration)
        {
            switch (configuration.Template.Name)
            {
                case ApiReferenceConfiguration.TemplateName.Default:
                    return new ApiReferenceDefaultTemplate(configuration);

                default:
                    throw new InvalidTemplateConfigurationException($"Unknown template '{configuration.Template.Name}'");
            }
        }
    }
}
