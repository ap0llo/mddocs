using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class PropertyReferenceExtensionsTest : DynamicCompilationTestBase
    {
        [Fact]
        public void ToMemberId_returns_the_expected_value()
        {
            // ARRANGE
            var cs = @"
                using System;

                namespace Grynwald.MdDocs.ApiReference.Test.TestData
                {
                    public class Class1
                    {
                        public int Property1 { get; set; }

                        public int this[int foo] { get { throw new NotImplementedException(); } }

                    }
                }
            ";


            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");

            // Property 1
            {
                var expectedMemberId = new PropertyId(
                    class1.ToTypeId(),
                    "Property1"
                );

                var propertyReference = class1.Properties.Single(p => p.Name == "Property1");

                // ACT
                var actualMemberId = propertyReference.ToMemberId();

                // ASSERT
                Assert.Equal(expectedMemberId, actualMemberId);
            }

            // Indexer
            {
                var expectedMemberId = new PropertyId(
                    class1.ToTypeId(),
                    "Item",
                    new[] { new SimpleTypeId("System", "Int32") }
                );

                var propertyReference = class1.Properties.Single(p => p.Name == "Item");

                // ACT
                var actualMemberId = propertyReference.ToMemberId();

                // ASSERT
                Assert.Equal(expectedMemberId, actualMemberId);
            }
        }

    }
}
