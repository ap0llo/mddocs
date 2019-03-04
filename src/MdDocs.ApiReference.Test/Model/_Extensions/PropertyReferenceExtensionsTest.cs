using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class PropertyReferenceExtensionsTest : TestBase
    {
        private static readonly TypeId s_TestClass_Properties = new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Properties");
        private static readonly TypeId s_System_Int32 = new SimpleTypeId("System", "Int32");

        private PropertyReference GetPropertyReference(string typeName, string propertyName)
        {
            return GetTypeDefinition(typeName)
               .Properties
               .First(x => x.Name == propertyName);
        }


        [Fact]
        public void ToMemberId_returns_the_expected_value_01()
        {
            // ARRANGE
            var expectedMemberId = new PropertyId(
                s_TestClass_Properties,
                nameof(TestClass_Properties.Property1)
            );
            var propertyReference = GetPropertyReference(expectedMemberId.DefiningType.Name, expectedMemberId.Name);

            // ACT
            var actualMemberId = propertyReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }

        [Fact]
        public void ToMemberId_returns_the_expected_value_02()
        {
            // ARRANGE
            var expectedMemberId = new PropertyId(
                s_TestClass_Properties,
                "Item",
                new [] { s_System_Int32 }
            );
            var propertyReference = GetPropertyReference(expectedMemberId.DefiningType.Name, expectedMemberId.Name);

            // ACT
            var actualMemberId = propertyReference.ToMemberId();

            // ASSERT
            Assert.Equal(expectedMemberId, actualMemberId);
        }        
    }
}
