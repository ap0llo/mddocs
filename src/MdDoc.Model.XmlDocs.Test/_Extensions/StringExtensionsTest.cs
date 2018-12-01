using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MdDoc.Model.XmlDocs;
using Xunit.Sdk;

namespace MdDoc.Model.XmlDocs.Test
{
    public class StringExtensionsTest
    {

        [Theory]
        [InlineData("foo", "foo")]
        [InlineData("  foo", "  foo")]
        [InlineData("foo  ", "foo  ")]
        [InlineData("  foo  ", "  foo  ")]
        [InlineData("\tfoo", "\tfoo")]
        [InlineData("\tfoo\t", "\tfoo\t")]
        [InlineData("foo\t", "foo\t")]
        [InlineData("\r\nfoo", "foo")]
        [InlineData("\r\nfoo\r\n", "foo\r\n")]
        [InlineData("foo\r\n", "foo\r\n")]
        [InlineData("\rfoo", "foo")]
        [InlineData("\rfoo\r", "foo\r")]
        [InlineData("foo\r", "foo\r")]
        [InlineData("\nfoo", "foo")]
        [InlineData("\nfoo\n", "foo\n")]
        [InlineData("foo\n", "foo\n")]
        [InlineData("\r\n\r\nfoo", "foo")]
        [InlineData("\r\n\r\nfoo\r\n\r\n", "foo\r\n")]
        [InlineData("foo\r\n\r\n", "foo\r\n")]
        [InlineData("\r\rfoo", "foo")]
        [InlineData("\r\rfoo\r\r", "foo\r")]
        [InlineData("foo\r\r", "foo\r")]
        [InlineData("\n\nfoo", "foo")]
        [InlineData("\n\nfoo\n\n", "foo\n")]
        [InlineData("foo\n\n", "foo\n")]
        [InlineData("   \r\nfoo", "foo")]
        [InlineData("   \r\nfoo  \r\n", "foo  \r\n")]
        [InlineData("foo  \r\n  ", "foo  \r\n")]
        [InlineData("   \rfoo", "foo")]
        [InlineData("   \rfoo  \r", "foo  \r")]
        [InlineData("foo  \r", "foo  \r")]
        [InlineData("   \nfoo", "foo")]
        [InlineData("   \nfoo  \n", "foo  \n")]
        [InlineData("foo  \n", "foo  \n")]
        [InlineData("   \r\n  \r\nfoo", "foo")]
        [InlineData("   \r\n  \r\nfoo   \r\n  \r\n", "foo   \r\n")]
        [InlineData("foo   \r\n  \r\n", "foo   \r\n")]
        [InlineData("   \r  \rfoo", "foo")]
        [InlineData("   \r  \rfoo   \r  \r", "foo   \r")]
        [InlineData("foo   \r  \r", "foo   \r")]
        [InlineData("   \n  \nfoo", "foo")]        
        [InlineData("   \n  \nfoo   \n  \n", "foo   \n")]        
        [InlineData("foo   \n  \n", "foo   \n")]        
        [InlineData("  foo  \r\n", "  foo  \r\n")]
        [InlineData("\r\nfoo  ", "foo  ")]
        public void TrimEmptyLines_returns_expected_value(string input, string expectedResult)
        {
            var actualResult = input.TrimEmptyLines();

            if(!expectedResult.Equals(actualResult))
            {
                throw new XunitException($"Unexpected result from {nameof(StringExtensions.TrimEmptyLines)}\r\n" +
                    $"Input:    \"{input.Replace("\n", "\\n").Replace("\r", "\\r")}\"\r\n" +
                    $"Expected: \"{expectedResult.Replace("\n", "\\n").Replace("\r", "\\r")}\"\r\n" +
                    $"Actual:   \"{actualResult.Replace("\n", "\\n").Replace("\r", "\\r")}\"");
            }
        }

    }
}
