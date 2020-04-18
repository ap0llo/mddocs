using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Grynwald.MdDocs.Common.Test.Configuration
{
    public class DocsConfigurationLoaderTest : IDisposable
    {
        private readonly TemporaryDirectory m_ConfigurationDirectory = new TemporaryDirectory();
        private readonly string m_ConfigurationFilePath;

        public DocsConfigurationLoaderTest()
        {
            m_ConfigurationFilePath = Path.Combine(m_ConfigurationDirectory, "mddocs.settings.json");

            // clear environment variables (might be set by previous test runs)
            var envVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (var key in envVars.Keys.Cast<string>().Where(x => x.StartsWith("MDDOCS__")))
            {
                Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);
            }
        }

        public void Dispose() => m_ConfigurationDirectory.Dispose();

        private void PrepareConfiguration(string key, object value)
        {
            var configRoot = new JObject();

            var currentConfigObject = new JObject();
            configRoot.Add(new JProperty("mddocs", currentConfigObject));

            var keySegments = key.Split(":");
            for (var i = 0; i < keySegments.Length; i++)
            {
                // last fragment => add value
                if (i == keySegments.Length - 1)
                {
                    if (value.GetType().IsArray)
                    {
                        value = JArray.FromObject(value);
                    }

                    currentConfigObject.Add(new JProperty(keySegments[i], value));

                }
                // create child configuration object
                else
                {
                    var newConfigObject = new JObject();
                    currentConfigObject.Add(new JProperty(keySegments[i], newConfigObject));
                    currentConfigObject = newConfigObject;
                }
            }

            var json = configRoot.ToString(Formatting.Indented);
            File.WriteAllText(m_ConfigurationFilePath, json);
        }


        /// <summary>
        /// Gets the assertions that must be true for the default configuration
        /// </summary>
        public static IEnumerable<object[]> DefaultConfigAssertions()
        {
            static object[] TestCase(Action<DocsConfiguration> assertion)
            {
                return new[] { assertion };
            }

            yield return TestCase(config => Assert.NotNull(config));

            yield return TestCase(config => Assert.NotNull(config.CommandLineHelp));
            yield return TestCase(config => Assert.True(config.CommandLineHelp.IncludeVersion));

            yield return TestCase(config => Assert.NotNull(config.ApiReference));

            yield return TestCase(config => Assert.NotNull(config.Markdown));
            yield return TestCase(config => Assert.Equal(MarkdownPreset.Default, config.Markdown.Preset));
        }

        [Theory]
        [MemberData(nameof(DefaultConfigAssertions))]
        public void Default_configuration_is_valid(Action<DocsConfiguration> assertion)
        {
            var defaultConfig = DocsConfigurationLoader.GetDefaultConfiguration();
            assertion(defaultConfig);
        }

        [Theory]
        [MemberData(nameof(DefaultConfigAssertions))]
        public void Empty_configuration_is_valid(Action<DocsConfiguration> assertion)
        {
            // ARRANGE           
            File.WriteAllText(m_ConfigurationFilePath, "{ }");

            // ACT
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath);

            // ASSERT
            assertion(config);
        }

        [Theory]
        [MemberData(nameof(DefaultConfigAssertions))]
        public void GetConfiguration_returns_default_configuration_if_config_file_does_not_exist(Action<DocsConfiguration> assertion)
        {
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath);
            assertion(config);
        }

        [Theory]
        [MemberData(nameof(DefaultConfigAssertions))]
        public void GetConfiguration_returns_default_configuration_if_config_file_path_is_empty(Action<DocsConfiguration> assertion)
        {
            var config = DocsConfigurationLoader.GetConfiguation("");
            assertion(config);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CommandLineHelp_IncludeVersion_can_be_set_in_configuration_file(bool includeVersion)
        {
            // ARRANGE            
            PrepareConfiguration("commandlinehelp:includeversion", includeVersion);

            // ACT
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath);

            // ASSERT
            Assert.NotNull(config.CommandLineHelp);
            Assert.Equal(includeVersion, config.CommandLineHelp.IncludeVersion);
        }

        private class TestClass1
        {
            [ConfigurationValue("mddocs:commandlinehelp:includeversion")]
            public bool? IncludeVersion { get; set; }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CommandLineHelp_IncludeVersion_can_be_set_through_settings_object(bool includeVersion)
        {
            // ARRANGE            
            var settings = new TestClass1() { IncludeVersion = includeVersion };

            // ACT
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath, settings);

            // ASSERT
            Assert.NotNull(config.CommandLineHelp);
            Assert.Equal(includeVersion, config.CommandLineHelp.IncludeVersion);
        }



        [Theory]
        [CombinatorialData]
        public void Markdown_preset_can_be_set_in_configuration_file(MarkdownPreset preset)
        {
            // ARRANGE            
            PrepareConfiguration("markdown:preset", preset.ToString());

            // ACT
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath);

            // ASSERT
            Assert.NotNull(config.Markdown);
            Assert.Equal(preset, config.Markdown.Preset);
        }

        private class TestClass2
        {
            [ConfigurationValue("mddocs:markdown:preset")]
            public string? Preset { get; set; }
        }

        [Theory]
        [CombinatorialData]
        public void Markdown_preset_can_be_set_through_settings_object(MarkdownPreset preset)
        {
            // ARRANGE            
            var settings = new TestClass2() { Preset = preset.ToString() };

            // ACT
            var config = DocsConfigurationLoader.GetConfiguation(m_ConfigurationFilePath, settings);

            // ASSERT
            Assert.NotNull(config.Markdown);
            Assert.Equal(preset, config.Markdown.Preset);
        }
    }
}
