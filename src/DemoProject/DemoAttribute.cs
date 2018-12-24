using System;

namespace DemoProject
{
    /// <summary>
    /// An example of an customa attribute class for methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DemoAttribute : Attribute
    {
    }
}
