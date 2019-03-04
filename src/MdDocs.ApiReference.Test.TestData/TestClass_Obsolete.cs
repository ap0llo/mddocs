using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// Test class for testing is obsolete attributes are processed correctly
    /// </summary>
    [Obsolete("This type is obsolete")]
    public class TestClass_Obsolete
    {
    }

    // obsolete attribute without message
    [Obsolete]
    public class TestClass_Obsolete2
    {
    }
}
