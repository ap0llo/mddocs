using System;
using System.Collections.Generic;
using System.Reflection;
using Grynwald.MdDocs.Common.Configuration;
using Xunit;

namespace Grynwald.MdDocs.Test
{
    public class CommandLineHelpOptionsTest
    {
        private static bool IsConfigurationProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(CommandLineHelpOptions.Verbose) => false,
                nameof(CommandLineHelpOptions.OutputDirectory) => false,
                nameof(CommandLineHelpOptions.MarkdownPreset) => false,
                nameof(CommandLineHelpOptions.AssemblyPath) => false,
                nameof(CommandLineHelpOptions.NoVersion) => false,
                nameof(CommandLineHelpOptions.IncludeVersion) => true,
                _ => throw new NotImplementedException()
            };
        }

        public static IEnumerable<object[]> ConfigurationProperties()
        {
            foreach (var property in typeof(CommandLineHelpOptions).GetProperties())
            {
                if (IsConfigurationProperty(property.Name))
                    yield return new[] { property.Name };
            }
        }

        public static IEnumerable<object[]> NonConfigurationProperties()
        {
            foreach (var property in typeof(CommandLineHelpOptions).GetProperties())
            {
                if (!IsConfigurationProperty(property.Name))
                    yield return new[] { property.Name };
            }
        }

        [Theory]
        [MemberData(nameof(ConfigurationProperties))]
        public void Configuration_Properties_have_a_configuration_value_attribute(string propertyName)
        {
            // all properties in commandline parameter classes should have a ConfigurationValueAttribute
            // so the value can be used in the configuration system.

            var property = typeof(CommandLineHelpOptions).GetProperty(propertyName)!;

            var attribute = property.GetCustomAttribute<ConfigurationValueAttribute>();

            Assert.NotNull(attribute);
            Assert.NotNull(attribute!.Key);
            Assert.StartsWith("mddocs:", attribute!.Key);
            Assert.NotEqual("mddocs:", attribute!.Key);
        }

        [Theory]
        [MemberData(nameof(NonConfigurationProperties))]
        public void Non_configuration_Properties_do_have_a_configuration_value_attribute(string propertyName)
        {
            // all properties in commandline parameter classes should have a ConfigurationValueAttribute
            // so the value can be used in the configuration system.

            var property = typeof(CommandLineHelpOptions).GetProperty(propertyName)!;

            var attribute = property.GetCustomAttribute<ConfigurationValueAttribute>();

            Assert.Null(attribute);
        }
    }
}
