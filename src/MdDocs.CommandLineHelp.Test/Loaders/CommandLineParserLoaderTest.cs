using System;
using Grynwald.MdDocs.CommandLineHelp.Loaders;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.Test.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="CommandLineParserLoader"/>
    /// </summary>
    public class CommandLineParserLoaderTest : CommandLineDynamicCompilationTestBase
    {
        public enum ApplicationType
        {
            SingleCommand,
            MultiCommand
        }


        private readonly ILogger m_Logger = NullLogger.Instance;


        private string GetClassAttributes(ApplicationType type)
        {
            switch (type)
            {
                case ApplicationType.SingleCommand:
                    return "";

                case ApplicationType.MultiCommand:
                    return @"[Verb(""command1"")]";

                default:
                    throw new NotImplementedException();
            }
        }

        private IParameterCollection GetParameterCollection(ApplicationDocumentation application, ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.SingleCommand:
                    return Assert.IsType<SingleCommandApplicationDocumentation>(application);

                case ApplicationType.MultiCommand:
                    var mutliCommandApplication = Assert.IsType<MultiCommandApplicationDocumentation>(application);
                    return Assert.Single(mutliCommandApplication.Commands);

                default:
                    throw new NotImplementedException();
            }
        }


        [Fact]
        public void Load_returns_an_instance_of_SingleCommandApplication_when_assembly_only_has_a_single_options_class()
        {
            // ARRANGE
            using var assembly = Compile(@"
                using System;
                
                public class MyOptionClass
                { }
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.IsType<SingleCommandApplicationDocumentation>(application);
        }

        [Fact]
        public void Load_returns_an_instance_of_MultiCommandApplication_when_assembly_only_has_a_classes_with_a_Verb_attribute()
        {
            // ARRANGE
            using var assembly = Compile(@"
                using System;
                using CommandLine;

                [Verb(""myVerb"")]
                public class MyOptionClass
                { }
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.IsType<MultiCommandApplicationDocumentation>(application);
        }

        [Fact]
        public void Application_name_is_loaded_from_AssemblyTitleAttribute_if_it_exists()
        {
            // ARRANGE
            var assemblyName = "ApplicationName";
            var assemblyTitle = "AssemblyTitle";

            using var assembly = Compile($@"
                using System;
                using System.Reflection;

                [assembly: AssemblyTitleAttribute(""{assemblyTitle}"")]


                public class MyOptionClass
                {{ }}
            ",
            assemblyName: assemblyName);

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.Equal(assemblyTitle, application.Name);
        }

        [Fact]
        public void Application_name_is_the_assembly_name_if_no_AssemblyTitleAttribute_exists()
        {
            // ARRANGE
            var assemblyName = "ApplicationName";
            using var assembly = Compile(@"
                using System;
                
                public class MyOptionClass
                { }
            ",
            assemblyName: assemblyName);

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.Equal(assemblyName, application.Name);
        }

        [Fact]
        public void Application_version_is_loaded_from_AssemblyInformationalVersionAttribute_if_it_exists()
        {
            // ARRANGE
            var informationalVersion = "some-version-string";
            var version = "1.2.3.0";

            using var assembly = Compile($@"
                using System;
                using System.Reflection;

                [assembly: AssemblyInformationalVersion(""{informationalVersion}"")]
                [assembly: AssemblyVersion(""{version}"")]

                public class MyOptionClass
                {{ }}
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.Equal(informationalVersion, application.Version);
        }

        [Fact]
        public void Application_version_is_the_assembly_version_if_no_AssemblyInformationalVersionAttribute_exists()
        {
            // ARRANGE
            var version = "1.2.3.0";
            using var assembly = Compile($@"
                using System;
                using System.Reflection;

                [assembly: AssemblyVersion(""{version}"")]

                public class MyOptionClass
                {{ }}
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.Equal(version, application.Version);
        }

        [Fact]
        public void Application_usage_is_loaded_correctly_for_single_command_applications()
        {
            // ARRANGE
            using var assembly = Compile($@"
                using System;
                using CommandLine.Text;

                [assembly: AssemblyUsage(""usage line 1"", ""usage line 2"")]

                public class MyOptionClass
                {{ }}
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.NotNull(application.Usage);
            Assert.Collection(application.Usage,
                line => Assert.Equal("usage line 1", line),
                line => Assert.Equal("usage line 2", line));
        }

        [Fact]
        public void Application_usage_is_loaded_correctly_for_multi_command_applications()
        {
            // ARRANGE
            using var assembly = Compile($@"
                using System;
                using CommandLine;
                using CommandLine.Text;

                [assembly: AssemblyUsage(""usage line 1"", ""usage line 2"")]

                [Verb(""command"")]
                public class MyOptionClass
                {{ }}
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.NotNull(application.Usage);
            Assert.Collection(application.Usage,
                line => Assert.Equal("usage line 1", line),
                line => Assert.Equal("usage line 2", line));
        }

        [Fact]
        public void Application_usage_is_null_if_assembly_does_not_have_a_AssemblyUsage_attribute()
        {
            // ARRANGE
            using var assembly = Compile($@"
                using System;
                using CommandLine;

                [Verb(""command"")]
                public class MyOptionClass
                {{ }}
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            Assert.Null(application.Usage);
        }

        [Fact]
        public void Commands_are_loaded_correctly()
        {
            // ARRANGE
            using var assembly = Compile(@"
                using System;
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"", HelpText = ""Some help text"")]
                public class Command2Options
                { }
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            var multiCommandApplication = Assert.IsType<MultiCommandApplicationDocumentation>(application);
            Assert.Collection(multiCommandApplication.Commands,
                command =>
                {
                    Assert.Equal("command1", command.Name);
                    Assert.Null(command.Description);
                },
                command =>
                {
                    Assert.Equal("command2", command.Name);
                    Assert.Equal("Some help text", command.Description);
                });
        }

        [Fact]
        public void Abstract_classes_are_ignored_when_loading_commands()
        {
            // ARRANGE
            using var assembly = Compile(@"
                using System;
                using CommandLine;

                [Verb(""command1"")]
                public abstract class Command1Options
                { }

                [Verb(""command2"")]
                public class Command2Options
                { }
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            var multiCommandApplication = Assert.IsType<MultiCommandApplicationDocumentation>(application);
            Assert.Collection(multiCommandApplication.Commands,
                command => Assert.Equal("command2", command.Name));
        }

        [Fact]
        public void Hidden_commands_are_ignored_when_loading_commands()
        {
            // ARRANGE
            using var assembly = Compile(@"
                using System;
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"", Hidden = true)]
                public class Command2Options
                { }
            ");

            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            var multiCommandApplication = Assert.IsType<MultiCommandApplicationDocumentation>(application);
            Assert.Collection(multiCommandApplication.Commands,
                command => Assert.Equal("command1", command.Name));
        }

        [Theory]
        [CombinatorialData]
        public void Named_parameters_are_loaded_correctly(ApplicationType applicationType)
        {
            // ARRANGE
            using var assembly = Compile($@"
                using System;
                using CommandLine;

                public enum SomeEnum
                {{
                    Value1,
                    Value2,
                    SomeOtherValue
                }}

                {GetClassAttributes(applicationType)}
                public class Command1Options
                {{

                    [Option(""option1"", HelpText = ""some help text"", Default = ""some default"")]
                    public string Option1Property {{ get; set; }}

                    [Option('x', Default = 23)]
                    public int Option2Property {{ get; set; }}

                    [Option('y', Default = true)]
                    public bool? Option3Property {{ get; set; }}

                    [Option(""option4"", Hidden = true)]
                    public string Option4Property {{ get; set; }}

                    [Option(""option5"", Required = true, MetaValue = ""PATH"")]
                    public string Option5Property {{ get; set; }}

                    [Option('z', ""option6"")]
                    public SomeEnum Option6Property {{ get; set; }}

                    [Option(""option7"", Default = SomeEnum.SomeOtherValue)]
                    public SomeEnum Option7Property {{ get; set; }}
                }}
            ");

            // ACT
            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            var parameterCollection = GetParameterCollection(application, applicationType);

            Assert.Empty(parameterCollection.SwitchParameters);
            Assert.Empty(parameterCollection.PositionalParameters);
            Assert.Collection(parameterCollection.NamedParameters,
                param =>
                {
                    Assert.Equal("option1", param.Name);
                    Assert.Null(param.ShortName);
                    Assert.Equal("some help text", param.Description);
                    Assert.False(param.Required);
                    Assert.Equal("some default", param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Null(param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Equal("option5", param.Name);
                    Assert.Null(param.ShortName);
                    Assert.Null(param.Description);
                    Assert.True(param.Required);
                    Assert.Null(param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Equal("PATH", param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Equal("option6", param.Name);
                    Assert.Equal("z", param.ShortName);
                    Assert.Null(param.Description);
                    Assert.False(param.Required);
                    Assert.Null(param.DefaultValue);
                    Assert.NotNull(param.AcceptedValues);
                    Assert.Equal(3, param.AcceptedValues!.Count);
                    Assert.Contains("Value1", param.AcceptedValues);
                    Assert.Contains("Value2", param.AcceptedValues);
                    Assert.Contains("SomeOtherValue", param.AcceptedValues);
                    Assert.Null(param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Equal("option7", param.Name);
                    Assert.Null(param.ShortName);
                    Assert.Null(param.Description);
                    Assert.False(param.Required);
                    Assert.Equal("SomeOtherValue", param.DefaultValue);
                    Assert.NotNull(param.AcceptedValues);
                    Assert.Equal(3, param.AcceptedValues!.Count);
                    Assert.Contains("Value1", param.AcceptedValues);
                    Assert.Contains("Value2", param.AcceptedValues);
                    Assert.Contains("SomeOtherValue", param.AcceptedValues);
                    Assert.Null(param.ValuePlaceHolderName);
                },

                param =>
                {
                    Assert.Null(param.Name);
                    Assert.Equal("x", param.ShortName);
                    Assert.Null(param.Description);
                    Assert.False(param.Required);
                    Assert.Equal("23", param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Null(param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Null(param.Name);
                    Assert.Equal("y", param.ShortName);
                    Assert.Null(param.Description);
                    Assert.False(param.Required);
                    Assert.Equal("true", param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Null(param.ValuePlaceHolderName);
                });

            // Hidden parameters must be skipped
            Assert.DoesNotContain(parameterCollection.NamedParameters, param => param.Name == "option4");
        }

        [Theory]
        [CombinatorialData]
        public void Switch_parameters_are_loaded_correctly(ApplicationType applicationType)
        {
            // ARRANGE
            using var assembly = Compile($@"
                using System;
                using CommandLine;

                {GetClassAttributes(applicationType)}
                public class Command1Options
                {{
                    [Option(""option1"", HelpText = ""some help text"")]
                    public bool Option1Property {{ get; set; }}

                    [Option('x')]
                    public bool Option2Property {{ get; set; }}

                    [Option(""option3"", Hidden = true)]
                    public bool Option3Property {{ get; set; }}

                    [Option('y', ""option4"")]
                    public bool Option4Property {{ get; set; }}
                }}

            ");

            // ACT
            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);

            // ASSERT
            Assert.NotNull(application);
            var parameterCollection = GetParameterCollection(application, applicationType);

            Assert.Empty(parameterCollection.NamedParameters);
            Assert.Empty(parameterCollection.PositionalParameters);
            Assert.Collection(parameterCollection.SwitchParameters,
                param =>
                {
                    Assert.Equal("option1", param.Name);
                    Assert.Null(param.ShortName);
                    Assert.Equal("some help text", param.Description);
                },
                param =>
                {
                    Assert.Equal("option4", param.Name);
                    Assert.Equal("y", param.ShortName);
                },
                param =>
                {
                    Assert.Null(param.Name);
                    Assert.Equal("x", param.ShortName);
                });

            // Hidden parameters must be skipped
            Assert.DoesNotContain(parameterCollection.NamedParameters, param => param.Name == "option3");
        }

        [Theory]
        [CombinatorialData]
        public void Positional_parameters_are_loaded_correctly(ApplicationType applicationType)
        {
            // ARRANGE            
            using var assembly = Compile($@"
                using CommandLine;

                public enum SomeEnum
                {{
                    Value1,
                    Value2
                }}
    
                {GetClassAttributes(applicationType)}
                public class CommandOptions
                {{
                    [Value(0, HelpText = ""some help text"")]
                    public string Value1 {{ get; set; }}

                    [Value(1, Default = ""some default"")]
                    public string Value2 {{ get; set; }}

                    [Value(2, Required = true, MetaName = ""Informational Name"", MetaValue = ""PATH"")]
                    public SomeEnum Value3 {{ get; set; }}

                    [Value(3, Hidden = true)]
                    public string Value4 {{ get; set; }}
                }}
            ");

            // ACT
            var sut = new CommandLineParserLoader(m_Logger);

            // ACT
            var application = sut.Load(assembly);
            var parameterCollection = GetParameterCollection(application, applicationType);

            // ASSERT
            Assert.Empty(parameterCollection.NamedParameters);
            Assert.Empty(parameterCollection.SwitchParameters);
            Assert.Collection(parameterCollection.PositionalParameters,
                param =>
                {
                    Assert.Equal(0, param.Position);
                    Assert.Equal("some help text", param.Description);
                    Assert.False(param.Required);
                    Assert.Null(param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Null(param.InformationalName);
                    Assert.Null(param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Equal(1, param.Position);
                    Assert.Null(param.Description);
                    Assert.False(param.Required);
                    Assert.Equal("some default", param.DefaultValue);
                    Assert.Null(param.AcceptedValues);
                    Assert.Null(param.InformationalName);
                    Assert.Null(param.ValuePlaceHolderName);
                },
                param =>
                {
                    Assert.Equal(2, param.Position);
                    Assert.Null(param.Description);
                    Assert.True(param.Required);
                    Assert.Null(param.DefaultValue);
                    Assert.NotNull(param.AcceptedValues);
                    Assert.Collection(param.AcceptedValues,
                        str => Assert.Equal("Value1", str),
                        str => Assert.Equal("Value2", str));
                    Assert.Equal("Informational Name", param.InformationalName);
                    Assert.Equal("PATH", param.ValuePlaceHolderName);
                });

            Assert.DoesNotContain(parameterCollection.PositionalParameters, param => param.Position == 3);
        }
    }
}
