using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// TestClass for attributes
    /// </summary>
    [Obsolete("Marked as obsolete for testing purposes")]
    [Test]
    public static class TestClass_Attributes_ExtensionMethods
    {

        // defining an indexer makes the c# compiler emit a Extension attribute
        // this should be ignored when reading the type's attribute
        public static void Foo(this string str) => throw new NotImplementedException();
    }
}
