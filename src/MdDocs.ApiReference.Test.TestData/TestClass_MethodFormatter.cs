using System;
using System.Collections.Generic;
using System.IO;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    class TestClass_MethodFormatter
    {
        public TestClass_MethodFormatter()
        {
        }

        public TestClass_MethodFormatter(string foo)
        {
        }

        public TestClass_MethodFormatter(string foo, IEnumerable<string> bar)
        {
        }

        public TestClass_MethodFormatter(string foo, IEnumerable<string> bar, IList<DirectoryInfo> baz)
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



        public static TestClass_MethodFormatter operator +(TestClass_MethodFormatter left, TestClass_MethodFormatter right) => throw new NotImplementedException();

        public static implicit operator string(TestClass_MethodFormatter instance) => throw new NotImplementedException();
    }

    class TestClass_MethodFormatter<T>
    {
        public TestClass_MethodFormatter()
        {

        }

        public TestClass_MethodFormatter(string param)
        {

        }
    }
}
