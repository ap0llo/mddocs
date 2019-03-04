using System;

namespace DemoProject
{
    /// <summary>
    /// An example of an custom attribute class for methods
    /// </summary>
    /// <remarks>
    /// This page demonstrates how the generated documentation for attribute classes looks like.
    /// The same layout is used for all classes, see <see cref="DemoClass"/> for a more detailed description
    /// <para>
    /// What can be seen on this page is that the definition section includes a type's custom attribute
    /// including the attribute's constructor parameters, in this case <c>[AttributeUsage(AttributeTargets.Method)]</c>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class DemoAttribute : Attribute
    {
    }
}
