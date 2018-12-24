using System;

namespace DemoProject
{
    [Flags]
    public enum PropertyFlags
    {
        Flag1,
        Flag2,
        Flag3
    }

    /// <summary>
    /// An example of a custom attribute that is used to annotate a property with custom flags
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DemoPropertyAnnotationAttribute : Attribute
    {
        public DemoPropertyAnnotationAttribute(PropertyFlags flags)
        {

        }

    }
}
