namespace DemoProject
{
    /// <summary>
    /// An example of an user-defined enum meant
    /// </summary>
    /// <remarks>
    /// This page demonstrates how the generated documentation for structs looks like.
    /// The same layout is used for classes, structs, interfaces, and enums.
    /// <para>
    /// A more detailed description of type documentation can be seen in the documentation of <see cref="DemoClass"/>
    /// </para>
    /// <para>
    /// In contrast to other kinds of types, the definition section for enum does include the list
    /// of possible values of the enum including the numeric value.
    /// </para>
    /// <para>
    /// Another difference to other types is that for enum values, there won't be generated a separate
    /// page for every field
    /// </para>
    /// </remarks>
    /// <seealso cref="IDemoInterface"/>
    /// <seealso cref="IDemoInterface"/>
    /// <seealso cref="DemoStruct" />
    public enum DemoEnum
    {
        /// <summary>
        /// Enum value "Item1"
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        Item1,

        /// <summary>
        /// Enum value "Item2"
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        Item2,

        /// <summary>
        /// Yet another enum value
        /// </summary>
        /// <remarks>
        /// More detailed information can be provided in the Remarks section
        /// </remarks>
        AnotherItem
    }
}
