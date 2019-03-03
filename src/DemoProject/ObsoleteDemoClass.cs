using System;

namespace DemoProject
{
    /// <summary>
    /// A class meant to demonstrate how the generated documentation for a deprecated class looks like.
    /// </summary>
    /// <remarks>
    /// If a member is marked as obsolete using <see cref="ObsoleteAttribute"/> a warning is included in the generated documentation. 
    /// The documentation also includes the message specified for the obsolete attribute.
    /// </remarks>
    [Obsolete("This class is obsolete. Use DemoClass instead.")]
    public class ObsoleteDemoClass
    {
    }
}
