using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool IsExtensionMethod(this ICustomAttributeProvider member) =>
            member.CustomAttributes.Any(x => x.AttributeType.FullName == Constants.ExtensionAttributeFullName);

        public static bool IsObsolete(this ICustomAttributeProvider member, out string message)
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
