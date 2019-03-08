using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public static class FieldReferenceExtensions
    {
        public static MemberId ToMemberId(this FieldReference fieldReference)
        {
            return new FieldId(
                fieldReference.DeclaringType.ToTypeId(),
                fieldReference.Name
            );
        }
    }
}
