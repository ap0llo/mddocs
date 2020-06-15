using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class CustomAttributeExtensions
    {
        [return: MaybeNull]
        public static T GetPropertyValueOrDefault<T>(this CustomAttribute attribute, string name)
        {
            var value = attribute.Properties.SingleOrDefault(x => x.Name == name).Argument.Value;

            // no idea why the CustomAttributeArgument.Value returns the value wrapped in
            // another CustomAttributeArgument in some cases
            if (value is CustomAttributeArgument customAttributeArgumentValue)
            {
                var type = customAttributeArgumentValue.Type.Resolve();
                if(type?.IsEnum == true)
                {
                    var enumValue = Convert.ToInt64(customAttributeArgumentValue.Value);

                    value = type.GetEnumValues().SingleOrDefault(x => x.value == enumValue).name;
                }
                else
                {
                    value = customAttributeArgumentValue.Value;
                }
            }

            return (value == null) ? default : (T)value;
        }
    }
}
