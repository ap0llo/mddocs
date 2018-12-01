using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;
using Mono.Cecil;
using Xunit;

namespace MdDoc.Test.Model
{
    public class ModuleDocumentationTest : TestBase
    {
        [Fact]        
        public void Types_includes_expected_types()
        {
            //ARRANGE
            var expectedTypes = (new[]
            {
                typeof(TestClass_Constructors),
                typeof(TestClass_Fields),
                typeof(TestClass_Events),
                typeof(TestClass_GenericType<>),
                typeof(TestClass_Methods),
                typeof(TestClass_Operators),
                typeof(TestClass_Properties),
                typeof(TestStruct_Properties),
                typeof(TestStruct_Constructors),
                typeof(TestInterface_Properties),
                typeof(TestClass_Type),
                typeof(TestStruct_Type),
                typeof(TestInterface_Type),
                typeof(TestEnum_Type),
                typeof(TestClass_MethodOverloads),
                typeof(TestInterface_Events),
                typeof(TestClass_MultipleOperatorOverloads),
                typeof(TestClass_Attributes),
                typeof(TestStruct_Attributes),
                typeof(TestInterface_Attributes),
                typeof(TestEnum_Attributes),
                typeof(TestAttribute),
                typeof(TestClass_InterfaceImplementation),
                typeof(TestStruct_InterfaceImplementation),
                typeof(TestInterface_Inheritance),
                typeof(TestClass_Properties_CSharpDefinition),
                typeof(TestClass_XmlDocs)
            })
            .Distinct()
            .Select(GetTypeDefinition)
            .ToArray();

            // ACT
            var sut = m_AssemblyDocumentation.MainModuleDocumentation;
            var actualTypes = sut.Types;

            // ASSERT
            Assert.Equal(expectedTypes.Length, actualTypes.Count);
            Assert.All(
                expectedTypes,
                expectedType => Assert.Contains(actualTypes, x => x.Definition.Equals(expectedType))
            );
        }

        
        [Fact]
        public void Types_does_not_include_internal_types()
        {
            //ARRANGE
            var internalTypes = new[]
            {
                typeof(TestClass_InternalType)
            }
            .Select(GetTypeDefinition)
            .ToArray();

            // ACT
            var sut = m_AssemblyDocumentation.MainModuleDocumentation;
            var actualTypes = sut.Types;

            // ASSERT            
            Assert.All(
                internalTypes,
                internalType => Assert.DoesNotContain(actualTypes, x => x.Definition.Equals(internalType))
            );
        }


        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeReference = GetTypeName(typeof(TestClass_InternalType));
            var sut = m_AssemblyDocumentation.MainModuleDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeReference);

            // ASSERT
            Assert.Null(documentation);
        }


        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeName = GetTypeName(typeof(TestClass_Type));
            var sut = m_AssemblyDocumentation.MainModuleDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeName);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.Equal(typeName, documentation.Name);
        }

    }
}
