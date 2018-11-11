using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// TestClass for attributes
    /// </summary>
    [Obsolete("Marked as obsolete for testing purposes")]
    [Test]
    public enum TestEnum_Attributes
    {
        Value1, 
        Value2
    }
}
