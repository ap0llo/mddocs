using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class AssemblyDocumentationTest : DynamicCompilationTestBase
    {
        private AssemblyDocumentation GetAssemblyDocumentation(AssemblyDefinition assembly)
        {
            using var assemblySetDocumentation = AssemblySetDocumentation.FromAssemblyDefinitions(assembly);
            return assemblySetDocumentation.Assemblies.Single();
        }


        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    internal class InternalClass1
                    { }
                }
            ";

            using var assembly = Compile(cs);

            var typeId = assembly.MainModule.Types.Single(x => x.Name == "InternalClass1").ToTypeId();
            using var sut = GetAssemblyDocumentation(assembly);

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }

        [Fact]
        public void TryGetDocumentation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1
                    { }
                }
            ";

            using var assembly = Compile(cs);

            var typeId = assembly.MainModule.Types.Single(x => x.Name == "Class1").ToTypeId();
            using var sut = GetAssemblyDocumentation(assembly);

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
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
            using var sut = GetAssemblyDocumentation(assembly);

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
            using var sut = GetAssemblyDocumentation(assembly);

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
            using var sut = GetAssemblyDocumentation(assembly);

            // ACT / ASSERT          
            Assert.DoesNotContain(sut.Types, t => t.TypeId.Name == "NestedClass1");
            Assert.DoesNotContain(sut.Types, t => t.TypeId.Name == "NestedClass2");
        }
    }
}
