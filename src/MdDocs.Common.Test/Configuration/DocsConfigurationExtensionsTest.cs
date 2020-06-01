using Grynwald.MdDocs.Common.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Grynwald.MdDocs.Common.Test.Configuration
{
    public class DocsConfigurationExtensionsTest
    {
        [Theory]
        [CombinatorialData]
        public void GetSerializationOptions_can_load_options_for_all_preset_names(MarkdownPreset preset)
        {
            // ARRANGE

            var configMock = new Mock<IConfigurationWithMarkdownPresetSetting>(MockBehavior.Strict);
            configMock.Setup(x => x.MarkdownPreset).Returns(preset);

            // ACT
            var serializationOptions = configMock.Object.GetSerializationOptions(NullLogger.Instance);

            // ASSERT
            Assert.NotNull(serializationOptions);
        }
    }
}
