using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// Test class for operators
    /// </summary>
    public class TestClass_MultipleOperatorOverloads
    {
        /// <summary>
        /// Binary plus operator
        /// </summary>
        public static TestClass_MultipleOperatorOverloads operator +(TestClass_MultipleOperatorOverloads left, int right) { throw new NotImplementedException(); }

        /// <summary>
        /// Binary plus operator
        /// </summary>
        public static TestClass_MultipleOperatorOverloads operator +(TestClass_MultipleOperatorOverloads left, double right) { throw new NotImplementedException(); }
    }
}
