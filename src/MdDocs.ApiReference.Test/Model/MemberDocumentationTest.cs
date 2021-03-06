﻿using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public abstract class MemberDocumentationTest : DynamicCompilationTestBase, IDisposable
    {
        private readonly AssemblyDefinition m_Assembly;
        private readonly AssemblyDocumentation m_AssemblyDocumentation;

        public MemberDocumentationTest()
        {
            var cs = @"
                using System;

                namespace Namespace1.Namespace2
                {
                    public class Class1
                    {
                        public event EventHandler Event1;

                        public readonly Class1 Field1;

                        public int Property1 { get; set; }

                        public Class1()
                        { }

                        public Class1(string parameter)
                        { }

                        public void Method1() { }

                        public static Class1 operator +(Class1 other) => throw new NotImplementedException();

                    }

                    internal class InternalClass1
                    { }
                }
            ";

            m_Assembly = Compile(cs);

            m_AssemblyDocumentation = AssemblySetDocumentation.FromAssemblyDefinitions(m_Assembly)
                .Assemblies
                .Single();
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

            var typeDocumentation = m_AssemblyDocumentation.Types.Single();
            var sut = GetMemberDocumentationInstance(typeDocumentation);

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
            var sut = GetMemberDocumentationInstance(m_AssemblyDocumentation.Types.Single());

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
        }


        protected abstract MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation);
    }
}
