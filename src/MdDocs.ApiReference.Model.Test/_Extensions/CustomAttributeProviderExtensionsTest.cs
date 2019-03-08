using System;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0612 // Type or member is obsolete

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class CustomAttributeProviderExtensionsTest : TestBase
    {
        [Theory]
        [InlineData(typeof(TestClass_Obsolete), true, "This type is obsolete")]
        [InlineData(typeof(TestClass_Obsolete2), true, null)]
        [InlineData(typeof(TestClass_Type), false, null)]
        public void IsObsolete_returns_the_expected_value(Type type, bool expectedIsObsolete, string expectedMessage)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);

            // ACT
            var actualIsObsolete = typeDefinition.IsObsolete(out var actualMessage);

            // ASSERT
            Assert.Equal(expectedIsObsolete, actualIsObsolete);
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}

#pragma warning restore CS0612 // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete
