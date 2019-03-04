using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// TestClass for attributes
    /// </summary>
    [Obsolete("Marked as obsolete for testing purposes")]
    [Test]
    public readonly struct TestStruct_Attributes
    {
        // the readonly modified makes the C# compiler emit a IsReadOnly attribute
        // this should be ignored when reading the type's attribute
    }
}
