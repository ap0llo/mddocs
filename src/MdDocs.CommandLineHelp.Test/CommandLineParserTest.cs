using System;
using System.Collections.Generic;
using CommandLine;
using Xunit;
using Xunit.Sdk;

namespace Grynwald.MdDocs.CommandLineHelp.Test
{
    //TODO: Move to Loaders namespace
    /// <summary>
    /// Test class that verifies assumptions about the CommandLineParser library MdDocs makes are true
    /// </summary>
    public class CommandLineParserTest
    {
        private class Options1
        {
            [Option('x', "parameter1")]
            public bool Parameter1 { get; set; }
        }

        private class Options2
        {
            [Option('x', "parameter1", Required = true)]
            public bool Parameter1 { get; set; }
        }

        [Theory]
        [InlineData(new[] { "--parameter1" }, true)]
        [InlineData(new[] { "-x" }, true)]
        [InlineData(new string[0], false)]
        // value after parameter is ignored: 
        [InlineData(new[] { "--parameter1", "false" }, true)]
        [InlineData(new[] { "--parameter1", "true" }, true)]
        [InlineData(new[] { "-x", "false" }, true)]
        [InlineData(new[] { "-x", "true" }, true)]
        public void Boolean_options_are_treated_as_switch_parameters(string[] args, bool expected)
        {
            var parsed = Parse<Options1>(args);
            Assert.Equal(expected, parsed.Parameter1);
        }


        [Theory]
        [InlineData(new[] { "--parameter1" }, true)]
        [InlineData(new[] { "-x" }, true)]
        public void Boolean_options_do_not_require_a_value(string[] args, bool expected)
        {
            var parsed = Parse<Options2>(args);
            Assert.Equal(expected, parsed.Parameter1);
        }


        //TODO: Handling of bool? parameters

        private T Parse<T>(string[] args)
        {
            return Parser.Default
             .ParseArguments<T>(args)
             .MapResult(
                 (T opts) => opts,
                 (IEnumerable<Error> errors) => throw new XunitException($"CommandLine parsing failed: {String.Join(", ", errors)} ")
             );
        }
    }
}
