namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Enumeration of the supported list types for use with the <c><![CDATA[<list>]]></c> element.
    /// </summary>
    /// <remarks>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public enum ListType
    {
        None = 0,
        Bullet,
        Number,
        Table
    }
}
