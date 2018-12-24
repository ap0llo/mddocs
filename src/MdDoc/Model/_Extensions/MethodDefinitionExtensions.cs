using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public static class MethodDefinitionExtensions
    {

        public static bool IsExtensionMethod(this MethodDefinition method) =>
            method.CustomAttributes.Any(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute");

    }
}
