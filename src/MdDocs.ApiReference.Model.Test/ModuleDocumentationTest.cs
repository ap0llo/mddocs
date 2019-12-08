using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
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
                #pragma warning disable CS0612 // Type or member is obsolete
                #pragma warning disable CS0618 // Type or member is obsolete
                typeof(TestClass_Attributes),
                typeof(TestClass_Attributes_ExtensionMethods),
                typeof(TestStruct_Attributes),
                typeof(TestInterface_Attributes),
                typeof(TestEnum_Attributes),
                typeof(TestClass_Obsolete),
                typeof(TestClass_Obsolete2),
                #pragma warning restore CS0618 // Type or member is obsolete
                #pragma warning restore CS0612 // Type or member is obsolete
                typeof(TestAttribute),
                typeof(TestClass_InterfaceImplementation),
                typeof(TestStruct_InterfaceImplementation),
                typeof(TestInterface_Inheritance),
                typeof(TestClass_CSharpDefinition),
                typeof(TestClass_CSharpDefinition_ExtensionMethods),
                typeof(CSharpDefinitionTest1Attribute),
                typeof(CSharpDefinitionTest2Attribute),
                typeof(CSharpDefinitionTest3Attribute),
                typeof(CSharpDefinitionTest4Attribute),
                typeof(CSharpDefinitionTest5Attribute),
                typeof(CSharpDefinitionTestFlagsEnum),
                typeof(CSharpDefinitionTest_StaticClass),
                typeof(CSharpDefinitionTest_AbstractClass),
                typeof(CSharpDefinitionTest_SealedClass),
                typeof(CSharpDefinitionTest_GenericClass<>),
                typeof(CSharpDefinitionTest_GenericClass2<,>),
                typeof(CSharpDefinitionTest_ExtensionClass),
                typeof(CSharpDefinitionTest_ClassWithAttributes),
                typeof(CSharpDefinitionTest_ClassWithInterfaceImplementation),
                typeof(CSharpDefinitionTest_ClassWithMultipleInterfaceImplementations),
                typeof(CSharpDefinitionTest_ClassWithbaseTypeAndInterfaceImplementations),
                typeof(CSharpDefinitionTest_ClassWithBaseType),
                typeof(CSharpDefinitionTest_Interface),
                typeof(CSharpDefinitionTest_InterfaceWithMultipleInterfaceImplementations),
                typeof(CSharpDefinitionTest_Struct),
                typeof(CSharpDefinitionTest_StructWithMultipleInterfaceImplementations),
                typeof(CSharpDefinitionTest_ReadOnlyStruct),
                typeof(CSharpDefinitionTest_GenericInterface_Contravariant<>),
                typeof(CSharpDefinitionTest_GenericInterface_Covariant<>),
                typeof(CSharpDefinitionTestEnum),
                typeof(TestClass_XmlDocs<,>),
                typeof(TestClass_NoDocumentation),
                typeof(TestClass_RefParameters),
            })
            .Distinct()
            .Select(GetTypeDefinition)
            .ToArray();

            // ACT
            var sut = m_AssemblyDocumentation.Value.MainModuleDocumentation;
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
            var sut = m_AssemblyDocumentation.Value.MainModuleDocumentation;
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
            var typeId = GetTypeId(typeof(TestClass_InternalType));
            var sut = m_AssemblyDocumentation.Value.MainModuleDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }

        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeId = GetTypeId(typeof(TestClass_Type));
            var sut = m_AssemblyDocumentation.Value.MainModuleDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, ((TypeDocumentation)documentation).TypeId);
        }
    }
}
