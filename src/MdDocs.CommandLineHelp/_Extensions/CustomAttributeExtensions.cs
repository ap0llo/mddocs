using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp
{
    public static class CustomAttributeExtensions
    {
        public static T GetPropertyValueOrDefault<T>(this CustomAttribute attribute, string name)
        {
            var value = attribute.Properties.SingleOrDefault(x => x.Name == name).Argument.Value;

            // no idea why the CustomAttributeArgument.Value returns the value wrapped in
            // another CustomAttributeArgument in some cases
            if (value is CustomAttributeArgument)
            {
                value = ((CustomAttributeArgument)value).Value;
            }

            return (value == null) ? default : (T)value;
        }
    }
}
