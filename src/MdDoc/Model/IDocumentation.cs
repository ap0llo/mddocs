using Mono.Cecil;

namespace MdDoc.Model
{
    public interface IDocumentation
    {
        /// <summary>
        /// Tries to get the documentation for the specified type
        /// </summary>
        /// <returns>Returns documentation for the specified type or null if the type is unknown or not documented</returns>
        TypeDocumentation TryGetDocumentation(TypeName type);
    }
}
