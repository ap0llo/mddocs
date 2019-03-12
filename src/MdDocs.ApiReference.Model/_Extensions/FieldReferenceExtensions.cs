using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="FieldReference"/>.
    /// </summary>
    internal static class FieldReferenceExtensions
    {
        /// <summary>
        /// Gets the <see cref="MemberId"/> for the field.
        /// </summary>
        public static MemberId ToMemberId(this FieldReference fieldReference)
        {
            return new FieldId(
                fieldReference.DeclaringType.ToTypeId(),
                fieldReference.Name
            );
        }
    }
}
