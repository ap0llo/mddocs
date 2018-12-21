namespace MdDoc.Model
{
    public interface IDocumentation
    {
        /// <summary>
        /// Tries to get the documentation for the specified member
        /// </summary>
        /// <returns>Returns documentation for the specified member or null if it is unknown or not documented</returns>
        IDocumentation TryGetDocumentation(MemberId member);
    }
}
