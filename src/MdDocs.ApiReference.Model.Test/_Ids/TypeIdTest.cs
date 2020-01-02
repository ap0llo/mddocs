using System.Linq;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class TypeIdTest : DynamicCompilationTestBase
    {
        [Theory]
        [InlineData("int")]
        [InlineData("byte")]
        [InlineData("sbyte")]
        [InlineData("char")]
        [InlineData("decimal")]
        [InlineData("double")]
        [InlineData("float")]
        [InlineData("bool")]
        [InlineData("uint")]
        [InlineData("long")]
        [InlineData("ulong")]
        [InlineData("object")]
        [InlineData("short")]
        [InlineData("ushort")]
        [InlineData("string")]
        public void DisplayName_returns_the_CSharp_type_name_for_built_in_types(string expectedTypeName)
        {
            // ARRANGE
            var cs = $@"
                namespace Namespace1.Namespace2
                {{
                    public class Class1
                    {{
                        public {expectedTypeName} Property1 {{ get; set; }}
                    }}
                }}
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single()
                .PropertyType;

            // ACT
            var typeName = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeName.DisplayName);
        }

        [Theory]
        [InlineData("string[]")]
        [InlineData("Stream[]")]
        public void DisplayName_returns_the_expected_type_name_for_array_types(string expectedTypeName)
        {
            var cs = $@"
                using System.IO;

                namespace Namespace1.Namespace2
                {{
                    public class Class1
                    {{
                        public {expectedTypeName} Property1 {{ get; set; }}
                    }}
                }}
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single()
                .PropertyType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }

        [Theory]
        [InlineData("IEnumerable<string>")]
        [InlineData("IEnumerable<Stream>")]
        [InlineData("Dictionary<string, Stream>")]
        public void DisplayName_returns_the_expected_type_name_for_generic_types_with_type_arguments(string expectedTypeName)
        {
            var cs = $@"
                using System.IO;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {{
                    public class Class1
                    {{
                        public {expectedTypeName} Property1 {{ get; set; }}
                    }}
                }}
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single()
                .PropertyType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }

        [Theory]
        [InlineData("public IEnumerable<T1> Method1() => throw new NotImplementedException();", "IEnumerable<T1>")]
        [InlineData("public IEnumerable<T2> Method1() => throw new NotImplementedException();", "IEnumerable<T2>")]
        [InlineData("public Dictionary<T1, T2> Method1() => throw new NotImplementedException();", "Dictionary<T1, T2>")]
        [InlineData("public Dictionary<TKey, TValue> Method1<TKey, TValue>() => throw new NotImplementedException();", "Dictionary<TKey, TValue>")]
        public void DisplayName_returns_the_expected_type_name_for_generic_types_with_type_parameters(string methodDefinition, string expectedTypeName)
        {
            var cs = $@"
                using System;
                using System.Collections.Generic;

                namespace Namespace1.Namespace2
                {{
                    public class Class1<T1, T2>
                    {{
                        {methodDefinition}
                    }}
                }}
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1`2")
                .Methods
                .Single(p => p.Name == "Method1")
                .ReturnType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }


        [Theory]
        [InlineData("bool?")]
        public void DisplayName_returns_the_expected_type_name_for_nullable_types(string expectedTypeName)
        {
            var cs = $@"
                namespace Namespace1.Namespace2
                {{
                    public class Class1
                    {{
                        public {expectedTypeName} Property1 {{ get; set; }}
                    }}
                }}
            ";

            using var assembly = Compile(cs);

            var typeReference = assembly.MainModule.Types
                .Single(x => x.Name == "Class1")
                .Properties
                .Single()
                .PropertyType;

            // ACT
            var typeId = typeReference.ToTypeId();

            // ASSERT
            Assert.Equal(expectedTypeName, typeId.DisplayName);
        }
    }
}
