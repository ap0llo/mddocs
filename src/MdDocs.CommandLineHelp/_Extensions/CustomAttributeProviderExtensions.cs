using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class CustomAttributeProviderExtensions
    {
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
