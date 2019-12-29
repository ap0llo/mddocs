using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extensions methods for <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    internal static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Determines whether the specified member is an extension.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>Returns true if the member is an extension (i.e. has the <c>System.Runtime.CompilerServices.ExtensionAttribute</c> attribute).</returns>
        public static bool IsExtension(this ICustomAttributeProvider member) =>
            member.CustomAttributes.Any(x => x.AttributeType.FullName == Constants.ExtensionAttributeFullName);

        /// <summary>
        /// Checks whether the specified member is obsolete.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="message">If the member is obsolete and a message was specified, the message is saved to the <paramref name="message"/> parameter. Otherwise the value will be null.</param>
        /// <returns>Returns true if the member has been marked as obsolete using the <c>System.ObsoleteAttribute</c> attribute.</returns>
#if NETSTANDARD2_1 || NETCOREAPP3_0 || NETCOREAPP3_1
        public static bool IsObsolete(this ICustomAttributeProvider member, [NotNullWhen(true)]out string? message)
#else
        public static bool IsObsolete(this ICustomAttributeProvider member, out string? message)
#endif
        {
            var obsoleteAttribute = member.CustomAttributes.SingleOrDefault(x => x.AttributeType.FullName == Constants.ObsoleteAttributeFullName);

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
    }
}
