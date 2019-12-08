namespace DemoProject
{
    /// <summary>
    /// This class shows an example of how the documentation for nested types looks.
    /// </summary>
    public class NestedTypesDemo
    {
        /// <summary>
        /// This is an example of a nested class
        /// </summary>
        public class NestedClass1
        { }

        /// <summary>
        /// This is an example of a nested class that in turn contains nested types
        /// </summary>
        public class NestedClass2
        {
            /// <summary>
            /// This is an example of an nested class within a nested class
            /// </summary>
            public class NestedClass3
            { }
        }

        /// <summary>
        /// This is an example of a nested interface
        /// </summary>
        public interface NestedInterface1
        { }
    }
}
