using System;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public static class PropertyReferenceExtensions
    {
        public static MemberId ToMemberId(this PropertyReference property)
        {
            var parameters = property.Parameters.Count > 0
                ? property.Parameters.Select(p => p.ParameterType.ToTypeId()).ToArray()
                : Array.Empty<TypeId>();

            return new PropertyId(
                property.DeclaringType.ToTypeId(),
                property.Name,
                parameters
            );
        }
    }
}
