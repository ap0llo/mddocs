using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    
    public class TestClass_RefParameters
    {
        public bool MethodWithOutParameter(out string value) => throw new NotImplementedException();

        public bool MethodWithRefParameter(ref string value) => throw new NotImplementedException();
    }
}
