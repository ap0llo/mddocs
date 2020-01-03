using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class MethodOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override OverloadDocumentation GetOverloadDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Methods.First().Overloads.First();
        }


        [Fact]
        public void TypeParameters_is_empty_for_non_generic_method_01()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1<T1>
                    {
                        public void Method1(T1 parameter)
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentation.MainModuleDocumentation.Types.Single();


            // ACT
            var sut = typeDocumentation
                .Methods
                .Single()
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }

        [Fact]
        public void TypeParameters_is_empty_for_non_generic_method_02()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public void Method1()
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentation.MainModuleDocumentation.Types.Single();

            // ACT
            var sut = typeDocumentation
                .Methods
                .Single(m => m.Name == "Method1")
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }

        [Fact]
        public void TypeParameters_returns_expected_parameters_for_generic_methods()
        {
            // ARRANGE
            var cs = @"
                using System;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public T2 Method1<T1, T2>(T1 foo, T2 bar) => throw new NotImplementedException();                        
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            var typeDocumentation = assemblyDocumentation.MainModuleDocumentation.Types.Single();

            // ARRANGE / ACT
            var sut = typeDocumentation
                .Methods
                .Single()
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Equal(2, sut.TypeParameters.Count);
            Assert.Contains(sut.TypeParameters, x => x.Name == "T1");
            Assert.Contains(sut.TypeParameters, x => x.Name == "T2");
        }
    }
}
