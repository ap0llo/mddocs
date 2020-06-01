using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model.XmlDocs
{
    public class TypeIdBuilderTest
    {
        [Fact]
        public void ToTypeId_returns_expected_value_01()
        {
            // ARRANGE
            var expected = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1");

            // ACT
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Class1")
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_02()
        {
            // ARRANGE
            var expected = new SimpleTypeId(new NamespaceId("Namespace"), "Class1");

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace")
                .AddNameSegment("Class1")
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_03()
        {
            // ARRANGE
            var expected = new SimpleTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1");

            // ACT
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void ToTypeId_returns_expected_value_04(int arity)
        {
            // ARRANGE
            var expected = arity == 0
                ? (TypeId)new SimpleTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1")
                : (TypeId)new GenericTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1", arity);

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .SetArity(arity)
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_05()
        {
            // ARRANGE
            var outerType = new SimpleTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1");
            var expected = new SimpleTypeId(outerType, "NestedClass1");

            // ACT
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .BeginNestedType()
                .AddNameSegment("NestedClass1")
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_06()
        {
            // ARRANGE
            var outerType = new SimpleTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1");
            var nestedType1 = new SimpleTypeId(outerType, "NestedClass1");
            var nestedType2 = new SimpleTypeId(nestedType1, "NestedClass2");

            // ACT
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .BeginNestedType()
                .AddNameSegment("NestedClass1")
                .BeginNestedType()
                .AddNameSegment("NestedClass2")
                .ToTypeId();

            // ASSERT
            Assert.Equal(nestedType2, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_07()
        {
            // ARRANGE

            var expected = new GenericTypeId(new NamespaceId("MdDoc.Test.TestData"), "TestClass_GenericType", 1);

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("MdDoc")
                .AddNameSegment("Test")
                .AddNameSegment("TestData")
                .AddNameSegment("TestClass_GenericType")
                .SetArity(1)
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTypeId_returns_expected_value_08()
        {
            // ARRANGE
            var stringTypeId = new SimpleTypeId("System", "String");

            var expected = new GenericTypeInstanceId(new NamespaceId("Namespace1"), "Class1", new[] { stringTypeId });

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Class1")
                .SetTypeArguments(new[] { stringTypeId })
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetArity_implicitly_creates_a_nested_type()
        {
            // ARRANGE
            var outerType = new GenericTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1", 1);
            var expected = new SimpleTypeId(outerType, "NestedClass1");

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .SetArity(1)
                .AddNameSegment("NestedClass1")
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BeginNestedType_has_no_effect_if_not_followed_by_a_AddNameSegemtn_call()
        {
            // ARRANGE
            var expected = new GenericTypeId(new NamespaceId("Namespace1.Namespace2"), "Class1", 1);

            // ACT            
            var actual = TypeIdBuilder.Create()
                .AddNameSegment("Namespace1")
                .AddNameSegment("Namespace2")
                .AddNameSegment("Class1")
                .SetArity(1)
                .BeginNestedType()
                .ToTypeId();

            // ASSERT
            Assert.Equal(expected, actual);
        }
    }
}
