using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class MethodDocumentationTest : MemberDocumentationTest
    {
        [Fact]
        public void Name_returns_the_expected_value_for_generic_overloads()
        {
            // ARRANGE
            var cs = @"
                public class Classs1
                {
                    public void Method1() { }

                    public void Method1(string foo) { }

                    public void Method1<T>(T foo) { }
                }";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);

            // ACT
            var methods = assemblyDocumentation.MainModuleDocumentation
                .Types
                .Single()
                .Methods;

            // ASSERT
            var method = Assert.Single(methods, m => m.Name == "Method1");
            Assert.Equal(3, method.Overloads.Count);
        }

        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Methods.First();
        }
    }
}
