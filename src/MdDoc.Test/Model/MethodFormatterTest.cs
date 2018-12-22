using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;
using Xunit;

namespace MdDoc.Test.Model
{
    public class MethodFormatterTest: TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_MethodFormatter.Method1), "Method1()")]
        [InlineData(nameof(TestClass_MethodFormatter.Method2), "Method2()")]
        [InlineData(nameof(TestClass_MethodFormatter.Method3), "Method3(string)")]
        [InlineData(nameof(TestClass_MethodFormatter.Method4), "Method4(IDisposable)")]
        [InlineData(nameof(TestClass_MethodFormatter.Method5), "Method5<T>(string)")]
        [InlineData(nameof(TestClass_MethodFormatter.Method6), "Method6<T>(T)")]
        [InlineData(nameof(TestClass_MethodFormatter.Method7), "Method7<T1, T2>(T1, T2)")]
        public void GetSignature_returns_the_expected_result(string methodName, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_MethodFormatter))
                   .Methods
                   .Single(x => x.Name == methodName);

            var sut = MethodFormatter.Instance;

            // ACT
            var actualSignature = sut.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData(0, "TestClass_MethodFormatter()")]
        [InlineData(1, "TestClass_MethodFormatter(string)")]
        [InlineData(2, "TestClass_MethodFormatter(string, IEnumerable<string>)")]
        [InlineData(3, "TestClass_MethodFormatter(string, IEnumerable<string>, IList<DirectoryInfo>)")]
        public void GetSignature_returns_the_expected_result_for_constructors(int parameterCount, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_MethodFormatter))
                   .Methods
                   .Single(x => x.IsConstructor && x.Parameters.Count == parameterCount);

            var sut = MethodFormatter.Instance;

            // ACT
            var actualSignature = sut.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData("op_Addition", "Addition(TestClass_MethodFormatter, TestClass_MethodFormatter)")]
        [InlineData("op_Implicit", "Implicit(TestClass_MethodFormatter to string)")]
        public void GetSignature_returns_the_expected_result_for_operators(string methodName, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_MethodFormatter))
                   .Methods
                   .Single(x => x.Name == methodName);

            var sut = MethodFormatter.Instance;

            // ACT
            var actualSignature = sut.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

    }
}
