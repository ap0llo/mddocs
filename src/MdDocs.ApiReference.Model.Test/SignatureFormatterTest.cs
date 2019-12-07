using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class SignatureFormatterTest : TestBase
    {
        [Theory]
        [InlineData(nameof(TestClass_SignatureFormatter.Method1), "Method1()")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method2), "Method2()")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method3), "Method3(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method4), "Method4(IDisposable)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method5), "Method5<T>(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method6), "Method6<T>(T)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method7), "Method7<T1, T2>(T1, T2)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method8), "Method8(ConsoleColor?)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method9), "Method9(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method10), "Method10(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method11), "Method11(string)")]
        public void GetSignature_returns_the_expected_result(string methodName, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.Name == methodName);
            
            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData(nameof(TestClass_SignatureFormatter.Method1), "Method1()")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method2), "Method2()")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method3), "Method3(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method4), "Method4(IDisposable)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method5), "Method5<T>(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method6), "Method6<T>(T)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method7), "Method7<T1, T2>(T1, T2)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method8), "Method8(ConsoleColor?)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method9), "Method9(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method10), "Method10(string)")]
        [InlineData(nameof(TestClass_SignatureFormatter.Method11), "Method11(string)")]
        public void GetSignature_returns_the_expected_result_for_method_ids(string methodName, string expectedSignature)
        {
            // ARRANGE
            var methodId = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.Name == methodName)
                   .ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData(0, "TestClass_SignatureFormatter()")]
        [InlineData(1, "TestClass_SignatureFormatter(string)")]
        [InlineData(2, "TestClass_SignatureFormatter(string, IEnumerable<string>)")]
        [InlineData(3, "TestClass_SignatureFormatter(string, IEnumerable<string>, IList<DirectoryInfo>)")]
        public void GetSignature_returns_the_expected_result_for_constructors(int parameterCount, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.IsConstructor && x.Parameters.Count == parameterCount);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData(0, "TestClass_SignatureFormatter()")]
        [InlineData(1, "TestClass_SignatureFormatter(string)")]
        public void GetSignature_returns_the_expected_result_for_constructors_of_generic_types(int parameterCount, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_SignatureFormatter<>))
                   .Methods
                   .Single(x => x.IsConstructor && x.Parameters.Count == parameterCount);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData(0, "TestClass_SignatureFormatter()")]
        [InlineData(1, "TestClass_SignatureFormatter(string)")]
        [InlineData(2, "TestClass_SignatureFormatter(string, IEnumerable<string>)")]
        [InlineData(3, "TestClass_SignatureFormatter(string, IEnumerable<string>, IList<DirectoryInfo>)")]
        public void GetSignature_returns_the_expected_result_for_constructors_as_method_ids(int parameterCount, string expectedSignature)
        {
            // ARRANGE
            var methodId = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.IsConstructor && x.Parameters.Count == parameterCount)
                   .ToMethodId();
            
            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData(0, "TestClass_SignatureFormatter()")]
        [InlineData(1, "TestClass_SignatureFormatter(string)")]
        public void GetSignature_returns_the_expected_result_for_constructors_of_generic_types_as_method_ids(int parameterCount, string expectedSignature)
        {
            // ARRANGE
            var methodId = GetTypeDefinition(typeof(TestClass_SignatureFormatter<>))
                   .Methods
                   .Single(x => x.IsConstructor && x.Parameters.Count == parameterCount)
                   .ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }

        [Theory]
        [InlineData("op_Addition", "Addition(TestClass_SignatureFormatter, TestClass_SignatureFormatter)")]
        [InlineData("op_Implicit", "Implicit(TestClass_SignatureFormatter to string)")]
        public void GetSignature_returns_the_expected_result_for_operators(string methodName, string expectedSignature)
        {
            // ARRANGE
            var method = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.Name == methodName);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(method);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData("op_Addition", "Addition(TestClass_SignatureFormatter, TestClass_SignatureFormatter)")]
        [InlineData("op_Implicit", "Implicit(TestClass_SignatureFormatter to string)")]
        public void GetSignature_returns_the_expected_result_for_operators_as_method_ids(string methodName, string expectedSignature)
        {
            // ARRANGE
            var methodId = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Methods
                   .Single(x => x.Name == methodName)
                   .ToMethodId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(methodId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData(1, "Item[int]")]
        [InlineData(2, "Item[int, int]")]
        public void GetSignature_returns_the_expected_result_for_indexers(int paramterCount, string expectedSignature)
        {
            // ARRANGE
            var property = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Properties
                   .Single(x => x.Parameters.Count == paramterCount);

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(property);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }


        [Theory]
        [InlineData(1, "Item[int]")]
        [InlineData(2, "Item[int, int]")]
        public void GetSignature_returns_the_expected_result_for_indexers_as_property_id(int paramterCount, string expectedSignature)
        {
            // ARRANGE
            var propertyId = GetTypeDefinition(typeof(TestClass_SignatureFormatter))
                   .Properties
                   .Single(x => x.Parameters.Count == paramterCount)
                   .ToPropertyId();

            // ACT
            var actualSignature = SignatureFormatter.GetSignature(propertyId);

            // ASSERT
            Assert.Equal(expectedSignature, actualSignature);
        }
    }
}
