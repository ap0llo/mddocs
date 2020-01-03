using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class ModuleDocumentationTest : DynamicCompilationTestBase
    {
        private AssemblyDocumentation GetAssemblyDocumentation(AssemblyDefinition assemblyDefinition)
        {
            return new AssemblyDocumentation(assemblyDefinition, NullXmlDocsProvider.Instance, NullLogger.Instance);
        }


        [Fact]
        public void Types_includes_expected_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                public interface Interface1
                { }

                public enum Enum1
                {
                    Value1,
                    Value2
                }

                public struct Struct1
	            { }

                public class Class2
    	        {
                    public class NestedClass1
                    { }

                    public class NestedClass2
                    {
                        public class NestedClass3
                        { }
                    }
                }

            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.MainModuleDocumentation;

            var expectedTypes = new[]
            {
                new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"),
                new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1"),
                new SimpleTypeId(NamespaceId.GlobalNamespace, "Enum1"),
                new SimpleTypeId(NamespaceId.GlobalNamespace, "Struct1"),
                new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"),
                new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass1"),
                new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass2"),
                new SimpleTypeId(new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass2"), "NestedClass3")
            };

            // ACT
            var actualTypes = sut.Types;

            // ASSERT
            Assert.Equal(expectedTypes.Length, actualTypes.Count);
            Assert.All(
                expectedTypes,
                expectedType => Assert.Contains(actualTypes, x => x.TypeId.Equals(expectedType))
            );
        }

        [Fact]
        public void Types_does_not_include_internal_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                internal class Class2
                { }
       
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.MainModuleDocumentation;

            // ACT / ASSERT          
            Assert.DoesNotContain(sut.Types, t => t.TypeId.Name == "Class2");
        }

        [Fact]
        public void Types_does_not_include_non_public_nested_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    internal class NestedClass1
                    { }

                    class NestedClass2
                    { }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.MainModuleDocumentation;


            // ACT / ASSERT          
            Assert.DoesNotContain(sut.Types, t => t.TypeId.Name == "NestedClass1");
            Assert.DoesNotContain(sut.Types, t => t.TypeId.Name == "NestedClass2");
        }

        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                internal class Class2
                { }
       
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.MainModuleDocumentation;


            var typeId = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2");

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }

        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                public class Class2
                { }
       
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.MainModuleDocumentation;


            var typeId = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2");

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
        }
    }
}
