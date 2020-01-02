using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public abstract class OverloadDocumentationTest : DynamicCompilationTestBase
    {
        private readonly AssemblyDefinition m_Assembly;
        private readonly AssemblyDocumentation m_AssemblyDocumentation;

        public OverloadDocumentationTest()
        {
            var cs = @"
                using System;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {                      
                        public void Method1() => throw new NotImplementedException();

                        public static Class1 operator +(Class1 other) => throw new NotImplementedException();

                        public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                        public static Class1 operator -(Class1 other) => throw new NotImplementedException();

                        public static Class1 operator -(Class1 left, Class1 right) => throw new NotImplementedException();
                    }

                    internal class InternalClass1
                    { }
                }
            ";

            m_Assembly = Compile(cs);

            m_AssemblyDocumentation = new AssemblyDocumentation(m_Assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
        }

        public void Dispose()
        {
            m_AssemblyDocumentation.Dispose();
            m_Assembly.Dispose();
        }



        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeId = new SimpleTypeId("Namespace1.Namespace2", "InternalClass1");
            var sut = GetOverloadDocumentationInstance(m_AssemblyDocumentation.MainModuleDocumentation.Types.Single());

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }


        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeId = new SimpleTypeId("Namespace1.Namespace2", "Class1");
            var sut = GetOverloadDocumentationInstance(m_AssemblyDocumentation.MainModuleDocumentation.Types.Single());

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
        }


        protected abstract OverloadDocumentation GetOverloadDocumentationInstance(TypeDocumentation typeDocumentation);
    }
}
