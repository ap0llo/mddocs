using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.Common.Configuration;
using Xunit;

namespace Grynwald.MdDocs.Common.Test.Configuration
{
    /// <summary>
    /// Tests verifying assumption about the referenced version of the MarkdownGenerator library
    /// </summary>
    public class MarkdownGeneratorTests
    {
        [Theory]
        [CombinatorialData]
        public void MdSerializationOptions_supports_preset_name(MarkdownPreset preset)
        {
            // MdSerializationOptions.Presets.Get() is defined in MarkdownGenerator library but
            // MarkdownPreset is defined in this application.
            // This test ensures, a MdSerializationOptions instance can be retrieved for every value of MarkdownPreset
            // (Get() would throw PresetNotFoundException if name was unknown)
            var serializationOptions = MdSerializationOptions.Presets.Get(preset.ToString());
            Assert.NotNull(serializationOptions);

        }
    }
}
