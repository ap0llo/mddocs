using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Test
{
    public class ModuleDocumentationTest : TestBase
    {

        [Fact]        
        public void Types_includes_expected_types()
        {
            //ARRANGE
            var expectedTypes = new[]
            {
                typeof(TestClass_Constructors),
                typeof(TestClass_Fields),
                typeof(TestClass_Events),
                typeof(TestClass_GenericType<>),
                typeof(TestClass_Methods),
                typeof(TestClass_Operators),
                typeof(TestClass_Properties),
                typeof(TestClass_Type)
            }
            .Select(t => m_Module.GetTypes().Single(typeDef => typeDef.Name == t.Name))
            .ToArray();

            // ACT
            var sut = new ModuleDocumentation(m_Module);
            var actualTypes = sut.Types;

            // ASSERT
            Assert.Equal(expectedTypes.Length, actualTypes.Count);
            Assert.All(
                expectedTypes, 
                expectedType => Assert.Contains(actualTypes, x => x.TypeReference.Equals(expectedType))
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
            .Select(t => m_Module.GetTypes().Single(typeDef => typeDef.Name == t.Name))
            .ToArray();

            // ACT
            var sut = new ModuleDocumentation(m_Module);
            var actualTypes = sut.Types;

            // ASSERT            
            Assert.All(
                internalTypes,
                internalType => Assert.DoesNotContain(actualTypes, x => x.TypeReference.Equals(internalType))
            );
        }

    }
}
