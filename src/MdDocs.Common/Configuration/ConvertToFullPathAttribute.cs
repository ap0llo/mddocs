using System;

namespace Grynwald.MdDocs.Common.Configuration
{
    /// <summary>
    /// Attribute to indicate a property value is a path that should be converted to a full path
    /// </summary>
    /// <remarks>
    /// When a configuration object is loaded through <see cref="ConfigurationProvider"/>, the value of all properties
    /// annotated with <see cref="ConvertToFullPathAttribute"/> will be interpreted as a path relative to the configuration file
    /// location and be converted to a full path.
    /// </remarks>
    /// <seealso cref="ConfigurationProvider"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ConvertToFullPathAttribute : Attribute
    { }
}
