using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using Grynwald.MdDocs.Common.Configuration;
using Xunit;

namespace Grynwald.MdDocs.Test
{
    /// <summary>
    /// Tests verifying commandline parameter classes
    /// </summary>
    public class CommandLineTests
    {
        private static bool IsConfigurationProperty(Type type, string propertyName)
        {
            return (type.Name, propertyName) switch
            {
                (_, nameof(OptionsBase.Verbose)) => false,
                (_, nameof(OptionsBase.MarkdownPreset)) => true,
                (_, nameof(OptionsBase.ConfigurationFilePath)) => false,
                (nameof(CommandLineHelpOptions), nameof(OptionsBase.OutputDirectory)) => true,
                (nameof(CommandLineHelpOptions), nameof(CommandLineHelpOptions.AssemblyPath)) => false,
                (nameof(CommandLineHelpOptions), nameof(CommandLineHelpOptions.NoVersion)) => false,
                (nameof(CommandLineHelpOptions), nameof(CommandLineHelpOptions.IncludeVersion)) => true,
                (nameof(ApiReferenceOptions), nameof(CommandLineHelpOptions.AssemblyPath)) => false,
                (nameof(ApiReferenceOptions), nameof(OptionsBase.OutputDirectory)) => true,
                _ => throw new NotImplementedException()
            };
        }

        public static IEnumerable<Type> GetCommandLineParameterTypes()
        {
            return typeof(Program).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null);
        }

        public static IEnumerable<object[]> ConfigurationProperties()
        {
            foreach (var type in GetCommandLineParameterTypes())
            {
                foreach (var property in type.GetProperties())
                {
                    if (IsConfigurationProperty(type, property.Name))
                        yield return new object[] { type, property.Name };
                }
            }

        }

        public static IEnumerable<object[]> NonConfigurationProperties()
        {
            foreach (var type in GetCommandLineParameterTypes())
            {
                foreach (var property in type.GetProperties())
                {
                    if (!IsConfigurationProperty(type, property.Name))
                        yield return new object[] { type, property.Name };
                }
            }
        }

        [Theory]
        [MemberData(nameof(ConfigurationProperties))]
        public void Configuration_Properties_have_a_configuration_value_attribute(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName)!;

            var attribute = property.GetCustomAttribute<ConfigurationValueAttribute>();

            Assert.NotNull(attribute);
            Assert.NotNull(attribute!.Key);
            Assert.StartsWith("mddocs:", attribute!.Key);
            Assert.NotEqual("mddocs:", attribute!.Key);
        }

        [Theory]
        [MemberData(nameof(ConfigurationProperties))]
        public void Configuration_Properties_must_have_supported_types(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName)!;
            Assert.True(ConfigurationBuilderExtensions.IsSupportedPropertyType(property.PropertyType));
        }

        [Theory]
        [MemberData(nameof(NonConfigurationProperties))]
        public void Non_configuration_Properties_do_have_a_configuration_value_attribute(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName)!;

            var attribute = property.GetCustomAttribute<ConfigurationValueAttribute>();

            Assert.Null(attribute);
        }


        public static IEnumerable<object[]> CommandLineParameterTypes()
        {
            return GetCommandLineParameterTypes().Select(x => new[] { x });
        }

        [Theory]
        [MemberData(nameof(CommandLineParameterTypes))]
        public void All_commandline_parameter_classes_derive_from_OptionsBase(Type type)
        {
            Assert.True(typeof(OptionsBase).IsAssignableFrom(type));
        }

    }
}
