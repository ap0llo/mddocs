using Grynwald.MarkdownGenerator;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.Common.Configuration
{
    public enum MarkdownPreset
    {
        Default,
        MkDocs
    }

    public interface IConfigurationWithMarkdownPresetSetting
    {
        MarkdownPreset MarkdownPreset { get; }
    }

    public static class DocsConfigurationExtensions
    {
        public static MdSerializationOptions GetSerializationOptions(this IConfigurationWithMarkdownPresetSetting configuration, ILogger logger)
        {
            var presetName = configuration.MarkdownPreset.ToString();

            try
            {
                var preset = MdSerializationOptions.Presets.Get(presetName);
                logger.LogInformation($"Using preset '{presetName}' for generating markdown");
                return preset;
            }
            catch (PresetNotFoundException)
            {
                logger.LogInformation($"Preset '{presetName}' not found. Using default serialization options");
                return MdSerializationOptions.Presets.Default;
            }
        }
    }
}
