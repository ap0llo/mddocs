using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    public class TestClass_NestedTypes
    {
        /// <summary>
        /// Example of an inner class
        /// </summary>
        public class NestedClass1
        {
            public class NestedClass2 { }

            /// <summary>
            /// Method in a nested class
            /// </summary>
            public void Method1()
            { }
        }


        /// <summary>
        /// Example of an inner class
        /// </summary>
        public interface NestedInterface1
        {

        }

        internal class NestedClass3
        { }

        public class NestedClass4<T>
        {
            /// <summary>
            /// Method in a nested class
            /// </summary>
            public void Method1()
            { }
        }
    }


    public class TestClass_NestedTypes<T1>
    {
        /// <summary>
        /// Example of an inner class
        /// </summary>
        public class NestedClass1
        {
            public class NestedClass2<T2> { }
        }


        public void Method1(TestClass_NestedTypes.NestedClass4<string> parameter) => throw new NotImplementedException();

        public void Method2(TestClass_NestedTypes<string>.NestedClass1 parameter) => throw new NotImplementedException();

        /// <summary>
        /// Method 3
        /// </summary>
        /// <param name="parameter"></param>
        public void Method3(TestClass_NestedTypes<string>.NestedClass1.NestedClass2<int> parameter) => throw new NotImplementedException();
    }
}
