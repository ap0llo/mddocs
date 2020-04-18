using System;
using Grynwald.MdDocs.Common.Configuration;
using Xunit;

namespace Grynwald.MdDocs.Common.Test.Configuration
{
    /// <summary>
    /// Unit tests for <see cref="ConfigurationBuilderExtensions"/>
    /// </summary>
    public class ConfigurationBuilderExtensionsTest
    {
        private class TestSettingsClass1
        {
            [ConfigurationValue("root:Setting1")]
            public string? Setting1 { get; set; }

            public string? SomeProperty { get; set; }

            [ConfigurationValue("root:Setting2")]
            public bool Setting2 { get; set; }
        }

        [Fact]
        public void GetSettingsDictionary_returns_the_expected_configuration_values()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass1()
            {
                Setting1 = "value1",
                Setting2 = true
            };

            // ACT 
            var settingsDictionary = ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject);

            // ASSERT
            Assert.NotNull(settingsDictionary);
            Assert.Contains("root:Setting1", settingsDictionary.Keys);
            Assert.Contains("root:Setting2", settingsDictionary.Keys);

            Assert.Equal("value1", settingsDictionary["root:Setting1"]);
            Assert.Equal("True", settingsDictionary["root:Setting2"]);
        }

        private class TestSettingsClass2
        {
            [ConfigurationValue("")]
            public string? Setting1 { get; set; }
        }

        [Fact]
        public void GetSettingsDictionary_ignores_properties_when_the_configuration_key_is_empty()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass2();


            // ACT 
            var settingsDictionary = ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject);

            // ASSERT
            Assert.NotNull(settingsDictionary);
            Assert.Empty(settingsDictionary);
        }

        private class TestSettingsClass3
        {
            [ConfigurationValue("setting1")]
            public int Setting1 { get; set; }
        }

        [Fact]
        public void GetSettingsDictionary_throws_InvalidOperationException_is_property_type_is_not_string_or_bool()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass3();

            // ACT / ASSERT
            Assert.Throws<InvalidOperationException>(() => ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject));
        }

        private class TestSettingsClass4
        {
            [ConfigurationValue("setting1")]
            public string? Setting1 { set { } }
        }

        [Fact]
        public void GetSettingsDictionary_throws_InvalidOperationException_is_property_has_not_getter()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass4();

            // ACT / ASSERT
            Assert.Throws<InvalidOperationException>(() => ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject));
        }

        private class TestSettingsClass5
        {
            [ConfigurationValue("root:Setting1")]
            public string? Setting1 { get; set; }
        }

        [Fact]
        public void GetSettingsDictionary_ignores_properties_if_their_value_is_null()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass5() { Setting1 = null };

            // ACT 
            var settingsDictionary = ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject);

            // ASSERT
            Assert.NotNull(settingsDictionary);
            Assert.Empty(settingsDictionary);
        }


        private class TestSettingsClass6
        {
            [ConfigurationValue("root:Setting1")]
            public string? Setting1 { get; }


            public TestSettingsClass6(string setting1)
            {
                Setting1 = setting1;
            }
        }

        [Fact]
        public void GetSettingsDictionary_loads_values_from_readonly_properties()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass6("value1");

            // ACT 
            var settingsDictionary = ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject);

            // ASSERT
            Assert.NotNull(settingsDictionary);
            var kvp = Assert.Single(settingsDictionary);
            Assert.Equal("root:Setting1", kvp.Key);
            Assert.Equal("value1", kvp.Value);
        }

        private class TestSettingsClass7
        {
            [ConfigurationValue("setting1")]
            public bool? Setting1 { get; set; }

            [ConfigurationValue("setting2")]
            public bool? Setting2 { get; set; }

            [ConfigurationValue("setting3")]
            public bool? Setting3 { get; set; }
        }


        [Fact]
        public void GetSettingsDictionary_correctly_handles_nullable_bools()
        {
            // ARRANGE
            var settingsObject = new TestSettingsClass7()
            {
                Setting1 = true,
                Setting2 = false,
                Setting3 = null
            };

            // ACT 
            var settingsDictionary = ConfigurationBuilderExtensions.GetSettingsDictionary(settingsObject);

            // ASSERT
            Assert.NotNull(settingsDictionary);
            Assert.Contains("setting1", settingsDictionary.Keys);
            Assert.Contains("setting2", settingsDictionary.Keys);
            Assert.DoesNotContain("setting3", settingsDictionary.Keys);

            Assert.Equal("True", settingsDictionary["setting1"]);
            Assert.Equal("False", settingsDictionary["setting2"]);
        }


        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(bool?))]
        public void IsSupportedPropertyType_returns_true_for_supported_property_types(Type type)
        {
            Assert.True(ConfigurationBuilderExtensions.IsSupportedPropertyType(type));
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(object))]
        public void IsSupportedPropertyType_returns_false_for_unsupported_property_types(Type type)
        {
            Assert.False(ConfigurationBuilderExtensions.IsSupportedPropertyType(type));
        }
    }
}
