using System;
using System.Collections.Generic;
using System.IO;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    class TestClass_SignatureFormatter
    {
        public TestClass_SignatureFormatter()
        {
        }

        public TestClass_SignatureFormatter(string foo)
        {
        }

        public TestClass_SignatureFormatter(string foo, IEnumerable<string> bar)
        {
        }

        public TestClass_SignatureFormatter(string foo, IEnumerable<string> bar, IList<DirectoryInfo> baz)
        {
        }


        public int this[int parameter] => throw new NotImplementedException();

        public int this[int x, int y] => throw new NotImplementedException();


        public void Method1() { }

        public string Method2() => throw new NotImplementedException();

        public string Method3(string foo) => throw new NotImplementedException();

        public string Method4(IDisposable foo) => throw new NotImplementedException();

        public T Method5<T>(string foo) => throw new NotImplementedException();

        public object Method6<T>(T foo) => throw new NotImplementedException();

        public object Method7<T1, T2>(T1 foo, T2 bar) => throw new NotImplementedException();

        public int? Method8(ConsoleColor? parameter) => throw new NotImplementedException();

        public void Method9(ref string value) => throw new NotImplementedException();

        public void Method10(out string value) => throw new NotImplementedException();

        public void Method11(in string value) => throw new NotImplementedException();



        public static TestClass_SignatureFormatter operator +(TestClass_SignatureFormatter left, TestClass_SignatureFormatter right) => throw new NotImplementedException();

        public static implicit operator string(TestClass_SignatureFormatter instance) => throw new NotImplementedException();
    }

    class TestClass_SignatureFormatter<T>
    {
        public TestClass_SignatureFormatter()
        {

        }

        public TestClass_SignatureFormatter(string param)
        {

        }
    }
}
