using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Grynwald.MdDocs.Common.Test
{
    /// <summary>
    /// Tests for <see cref="EnumerableExtensions"/>
    /// </summary>
    public class EnumerableExtensionsTest
    {
        public class DuplicatesBy
        {
            [Fact]
            public void Returns_empty_enumerable_for_empty_enumerable()
            {
                // ARRANGE
                var enumerable = Enumerable.Empty<string>();

                // ACT 
                var duplicates = enumerable.DuplicatesBy(x => x);

                // ASSERT
                Assert.Empty(duplicates);
            }

            [Fact]
            public void Returns_empty_enumerable_if_there_are_no_duplicates()
            {
                // ARRANGE
                var enumerable = new[]
                {
                    "Value1",
                    "Value2",
                    "Value3"
                };

                // ACT 
                var duplicates = enumerable.DuplicatesBy(x => x);

                // ASSERT
                Assert.Empty(duplicates);
            }

            [Fact]
            public void Returns_duplicates_if_there_are_duplicates()
            {
                // ARRANGE
                var enumerable = new[]
               {
                    "Value1",
                    "Value2",
                    "Value3",
                    "Value1"
                };

                // ACT 
                var duplicates = enumerable.DuplicatesBy(x => x);

                // ASSERT
                Assert.Collection(
                    duplicates,
                    x => Assert.Equal("Value1", x)
                );
            }

            [Fact]
            public void Uses_the_specified_comparer()
            {
                // ARRANGE
                var enumerable = new[]
               {
                    "Value1",
                    "Value2",
                    "Value3",
                    "value1"
                };

                // ACT 
                var duplicates = enumerable.DuplicatesBy(x => x, StringComparer.OrdinalIgnoreCase);

                // ASSERT
                Assert.Collection(
                    duplicates,
                    x => Assert.Equal("value1", x, ignoreCase: true)
                );
            }
        }
    }
}
