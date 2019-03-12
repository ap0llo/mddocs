using System;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="PropertyReference"/>.
    /// </summary>
    internal static class PropertyReferenceExtensions
    {
        /// <summary>
        /// Gets the <see cref="MemberId"/> for the specified property.
        /// </summary>
        public static MemberId ToMemberId(this PropertyReference property) => property.ToPropertyId();

        /// <summary>
        /// Gets the <see cref="PropertyId"/> for the specified property.
        /// </summary>
        public static PropertyId ToPropertyId(this PropertyReference property)
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
