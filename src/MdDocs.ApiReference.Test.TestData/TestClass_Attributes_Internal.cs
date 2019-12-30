using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    internal class TestAttribute_Internal : Attribute
    {
    }

    /// <summary>
    /// TestClass for internal attributes
    /// </summary>
    [TestAttribute]
    [TestAttribute_Internal]
    public class TestClass_Attributes_Internal
    {        
    }
}
