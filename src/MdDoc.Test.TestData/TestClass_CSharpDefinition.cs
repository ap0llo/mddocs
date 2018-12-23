using System;
using System.Collections.Generic;
using System.IO;

namespace MdDoc.Test.TestData
{
    public class TestClass_CSharpDefinition
    {
        public string Field1;

        public static string Field2;

        public const string Field3 ="";

        public static readonly int Field4;


        public int Property1 { get; set; }

        public byte Property2 { get; set; }

        public string Property3 { get; }

        public string Property4 { get; private set; }

        public string Property5 { private get; set; }

        public Stream Property6 { get; }

        public IEnumerable<string> Property7 { get; }

        public static IEnumerable<string> Property8 { get; }

        public int this[object parameter] { get { throw new NotImplementedException(); } }

        public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }


        public TestClass_CSharpDefinition()
        { }

        public TestClass_CSharpDefinition(string parameter)
        { }

        static TestClass_CSharpDefinition()
        {
        }

        public void Method1() => throw new NotImplementedException();

        public string Method2() => throw new NotImplementedException();

        public string Method3(string param1, Stream param2) => throw new NotImplementedException();

        public static string Method4(string param1, Stream param2) => throw new NotImplementedException();        

        public static string Method5<TParam>(TParam parameter) => throw new NotImplementedException();

    }
}
