using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    /// <summary>
    /// Extensions methods for <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Determines whether the specified member is an extension.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>Returns true if the member is an extension (i.e. has the <c>System.Runtime.CompilerServices.ExtensionAttribute</c> attribute).</returns>
        public static bool IsExtension(this ICustomAttributeProvider member) =>
            member.CustomAttributes.Any(x => x.AttributeType.FullName == SystemTypeNames.ExtensionAttributeFullName);

        /// <summary>
        /// Checks whether the specified member is obsolete.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="message">If the member is obsolete and a message was specified, the message is saved to the <paramref name="message"/> parameter. Otherwise the value will be null.</param>
        /// <returns>Returns true if the member has been marked as obsolete using the <c>System.ObsoleteAttribute</c> attribute.</returns>
        public static bool IsObsolete(this ICustomAttributeProvider member, out string? message)
        {
            var obsoleteAttribute = member.CustomAttributes.SingleOrDefault(x => x.AttributeType.FullName == SystemTypeNames.ObsoleteAttributeFullName);

            if (obsoleteAttribute == null)
            {
                message = default;
                return false;
            }
            else
            {
                message = obsoleteAttribute.HasConstructorArguments ? obsoleteAttribute.ConstructorArguments.First().Value?.ToString() : default;
                return true;
            }
        }


        public static CustomAttribute GetAttribute(this ICustomAttributeProvider definition, string name)
        {
            return definition
                    .CustomAttributes
                    .Single(a => a.AttributeType.FullName == name);
        }

        public static CustomAttribute GetAttributeOrDefault(this ICustomAttributeProvider definition, string name)
        {
            return definition
                    .CustomAttributes
                    .SingleOrDefault(a => a.AttributeType.FullName == name);
        }

        public static bool HasAttribute(this ICustomAttributeProvider definiton, string name)
        {
            return definiton.CustomAttributes.Any(x => x.AttributeType.FullName == name);
        }

        public static IEnumerable<T> WithAttribute<T>(this IEnumerable<T> definitons, string name) where T : ICustomAttributeProvider
        {
            return definitons.Where(x => x.HasAttribute(name));
        }
    }
}
