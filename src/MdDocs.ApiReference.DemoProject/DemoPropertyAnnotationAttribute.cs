using System;

namespace DemoProject
{
    /// <summary>
    /// An example of a "flag" enum
    /// </summary>
    /// <remarks>
    /// <see cref="DemoPropertyFlags"/> serves two purposes:
    /// <para>
    /// On the one hand it showcases the generated documentation for "flag" enums.
    /// For flag enums, the numeric values for the possible enum values are represented
    /// as hexadecimal numbers in contrast to decimal numbers for other enum types.
    /// </para>
    /// <para>
    /// Furthermore, this enum is used in <see cref="DemoPropertyAnnotationAttribute"/> and demonstrates
    /// how custom attributes are included in the documentation for properties (see <see cref="DemoClass.Property2"/>)
    /// </para>
    /// </remarks>    
    /// <seealso cref="DemoPropertyAnnotationAttribute"/>
    /// <seealso cref="DemoClass.Property2"/>
    [Flags]
    public enum DemoPropertyFlags
    {
        /// <summary>
        /// Example of an enum value
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        Flag1 = 0x01,

        /// <summary>
        /// Example of an enum value
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        Flag2 = 0x02,

        /// <summary>
        /// Example of an enum value
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        Flag3 = 0x04
    }

    /// <summary>
    /// An example of a custom attribute that is used to annotate a property with custom flags.
    /// </summary>
    /// <remarks>
    /// <see cref="DemoPropertyAnnotationAttribute"/> showcases the generated documentation for a custom attribute class
    /// (in this case a attribute applicable to properties).
    /// The same layout is used for all classes, see <see cref="DemoClass"/> for a more detailed description.
    /// </remarks>
    /// <seealso cref="DemoClass.Property2" />
    [AttributeUsage(AttributeTargets.Property)]
    public class DemoPropertyAnnotationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DemoPropertyAnnotationAttribute"/>
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        /// <param name="flags">The flags to annotate the property with</param>
        public DemoPropertyAnnotationAttribute(DemoPropertyFlags flags)
        {

        }
    }
}
