using System;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// TestClass for attributes
    /// </summary>
    [Obsolete("Marked as obsolete for testing purposes")]
    [Test]
    public class TestClass_Attributes
    {
        // defining an indexer makes the c# compiler emit a DefaultMember attribute
        // this should be ignored when reading the type's attribute
        public int this[int param] => throw new NotImplementedException();
    }
}
