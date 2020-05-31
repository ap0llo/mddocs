using Grynwald.MdDocs.Common.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.Common.Test.Configuration
{
    public class DocsConfigurationExtensionsTest
    {
        [Theory]
        [CombinatorialData]
        public void GetSerializationOptions_can_load_options_for_all_preset_names(MarkdownPreset presetName)
        {
            // ARRANGE
            var config = new DocsConfiguration()
            {
                Markdown = new MarkdownConfiguration()
                {
                    Preset = presetName
                }
            };

            // ACT
            var serializationOptions = config.GetSerializationOptions(NullLogger.Instance);

            // ASSERT
            Assert.NotNull(serializationOptions);
        }
    }
}
