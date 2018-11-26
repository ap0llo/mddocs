using System;
using MdDoc.Test.TestData;
using System.Linq;
using Xunit;

namespace MdDoc.Test.Model
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Properties)).Properties.First();
        }


        [Theory]
        [InlineData("Property1", 0, @"public int Property1 { get; set; }")]
        [InlineData("Property2", 0, @"public byte Property2 { get; set; }")]
        [InlineData("Property3", 0, @"public sbyte Property3 { get; set; }")]
        [InlineData("Property4", 0, @"public char Property4 { get; set; }")]
        [InlineData("Property5", 0, @"public decimal Property5 { get; set; }")]
        [InlineData("Property6", 0, @"public double Property6 { get; set; }")]
        [InlineData("Property7", 0, @"public float Property7 { get; set; }")]
        [InlineData("Property8", 0, @"public bool Property8 { get; set; }")]
        [InlineData("Property9", 0, @"public uint Property9 { get; set; }")]
        [InlineData("Property10", 0, @"public long Property10 { get; set; }")]
        [InlineData("Property11", 0, @"public ulong Property11 { get; set; }")]
        [InlineData("Property12", 0, @"public object Property12 { get; set; }")]
        [InlineData("Property13", 0, @"public short Property13 { get; set; }")]
        [InlineData("Property14", 0, @"public ushort Property14 { get; set; }")]
        [InlineData("Property15", 0, @"public string Property15 { get; set; }")]
        [InlineData("Property16", 0, @"public string Property16 { get; }")]
        [InlineData("Property17", 0, @"public string Property17 { get; }")]
        [InlineData("Property18", 0, @"public string Property18 { set; }")]
        [InlineData("Property19", 0, @"public Stream Property19 { get; }")]
        [InlineData("Item", 1, @"public int this[object parameter] { get; }")]
        [InlineData("Item", 2, @"public int this[object parameter1, Stream parameter2] { get; }")]
        public void CSharpDefinition_returns_the_expected_definition(string propertyName, int parameterCount, string expected)
        {
            // ARRANGE
            var sut = GetTypeDocumentation(typeof(TestClass_Properties_CSharpDefinition))
                .Properties
                .Single(p => p.Name == propertyName && p.Definition.Parameters.Count == parameterCount);

            // ACT
            var actual = sut.CSharpDefinition;

            // ASSERT
            Assert.Equal(expected, actual);
        }
    }
}
