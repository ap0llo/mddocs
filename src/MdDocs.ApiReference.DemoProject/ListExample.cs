namespace DemoProject
{
    /// <summary>
    /// Example class to showcase the different supported list formats.
    /// </summary>
    /// <remarks>
    /// To create a list, use the <c>list</c> tag. Using this tag, bullet lists, numbered lists and two-column tables can be created.
    /// <para>
    ///     To create a bullet list, use the following syntax:
    ///     <code language="xml"><![CDATA[
    ///         <list type="bullet">
    ///             <item>
    ///                 <description>Item 1</description>
    ///             </item>
    ///             <item>
    ///                 <description>Item 2</description>
    ///             </item>
    ///         </list>
    ///     ]]></code>
    ///     which is rendered as this list
    ///      <list type="bullet">
    ///             <item>
    ///                 <description>Item 1</description>
    ///             </item>
    ///             <item>
    ///                 <description>Item 2</description>
    ///             </item>
    ///         </list>
    /// </para>
    /// <para>
    ///     By changing the type of the list to "number"
    ///     <code language="xml"><![CDATA[
    ///         <list type="number">
    ///             <item>
    ///                 <description>Item 1</description>
    ///             </item>
    ///             <item>
    ///                 <description>Item 2</description>
    ///             </item>
    ///         </list>
    ///     ]]></code>
    ///     a numbered list will be rendered:
    ///     <list type="number">
    ///            <item>
    ///                <description>Item 1</description>
    ///            </item>
    ///            <item>
    ///                <description>Item 2</description>
    ///            </item>
    ///        </list>
    /// </para>
    /// <para>
    ///     Both numbered and bullet list also support items that specify both a <c>description</c> and a <c>term</c>:
    ///     <code language="xml"><![CDATA[
    ///         <list type="number">
    ///             <item>
    ///                 <term>Term 1</term>
    ///                 <description>Item 1</description>
    ///             </item>
    ///             <item>
    ///                 <term>Term 2</term>
    ///                 <description>Item 2</description>
    ///             </item>
    ///         </list>
    ///     ]]></code>
    ///     
    ///     In this case, the value of <c>term</c> will be rendered as bold prefix for the description:
    ///     <list type="number">
    ///         <item>
    ///             <term>Term 1</term>
    ///             <description>Item 1</description>
    ///         </item>
    ///         <item>
    ///             <term>Term 2</term>
    ///             <description>Item 2</description>
    ///         </item>
    ///     </list>
    /// </para>
    /// <para>
    ///     By changing the type of the list to <c>table</c>, the list will be rendered as two-column table.
    ///     To specify the header row of the table , use <c>listheader</c>
    ///     <code language="xml"><![CDATA[
    ///         <list type="number">
    ///             <listheader>
    ///                 <term>Term</term>
    ///                 <description>Description</description>
    ///             </listheader>
    ///             <item>
    ///                 <term>Row 1, Column 1</term>
    ///                 <description>Row 1, Column 2</description>
    ///             </item>
    ///             <item>
    ///                 <term>Row 2, Column 1</term>
    ///                 <description>Row 2, Column 2</description>
    ///             </item>
    ///         </list>
    ///     ]]></code>
    ///     
    ///     In this case, the value of <c>term</c> will be rendered as bold prefix for the description:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Term</term>
    ///             <description>Description</description>
    ///         </listheader>
    ///         <item>
    ///             <term>Row 1, Column 1</term>
    ///             <description>Row 1, Column 2</description>
    ///         </item>
    ///         <item>
    ///             <term>Row 2, Column 1</term>
    ///             <description>Row 2, Column 2</description>
    ///         </item>
    ///     </list>
    /// </para>
    /// </remarks>
    public class ListExample
    {
    }
}
