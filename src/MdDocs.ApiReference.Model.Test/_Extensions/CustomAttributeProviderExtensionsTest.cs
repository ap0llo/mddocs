using System.Linq;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class CustomAttributeProviderExtensionsTest : DynamicCompilationTestBase
    {
        [Fact]
        public void IsObsolete_returns_the_expected_value()
        {
            // ARRANGE
            var cs = @"
                using System;

                [Obsolete(""This type is obsolete"")]  
                public class Class1
                { }
                
                [Obsolete]  
                public class Class2
                { }

                public class Class3
                {
                }           
            ";

            using var assembly = Compile(cs);


            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var class2 = assembly.MainModule.Types.Single(x => x.Name == "Class2");
            var class3 = assembly.MainModule.Types.Single(x => x.Name == "Class3");

            // obsolete attribute with message
            {
                // ACT
                var actualIsObsolete = class1.IsObsolete(out var actualMessage);

                // ASSERT
                Assert.True(actualIsObsolete);
                Assert.Equal("This type is obsolete", actualMessage);
            }
            // obsolete attribute without message
            {
                // ACT
                var actualIsObsolete = class2.IsObsolete(out var actualMessage);

                // ASSERT
                Assert.True(actualIsObsolete);
                Assert.Null(actualMessage);
            }
            // No obsolete attribute
            {
                // ACT
                var actualIsObsolete = class3.IsObsolete(out var actualMessage);

                // ASSERT
                Assert.False(actualIsObsolete);
                Assert.Null(actualMessage);
            }
        }
    }
}

