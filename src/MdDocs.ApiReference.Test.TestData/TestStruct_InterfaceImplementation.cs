using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    public struct TestStruct_InterfaceImplementation : IDisposable, TestInterface_Type
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
