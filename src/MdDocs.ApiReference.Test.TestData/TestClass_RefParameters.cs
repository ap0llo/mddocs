using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    
    public class TestClass_RefParameters
    {
        public bool Method1(out string value) => throw new NotImplementedException();

        public bool Method2(ref string value) => throw new NotImplementedException();

        public bool Method3(ref string[] value) => throw new NotImplementedException();
    }
}
