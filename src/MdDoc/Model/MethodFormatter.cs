using System.Linq;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model
{
    sealed class MethodFormatter
    {
        public static readonly MethodFormatter Instance = new MethodFormatter();

        
        private MethodFormatter()
        { }


        public string GetSignature(MethodDefinition method)
        {
            var signatureBuilder = new StringBuilder();

            if(method.IsConstructor)
            {
                signatureBuilder.Append(method.DeclaringType.Name);
            }
            else
            {
                signatureBuilder.Append(method.Name);
            }

            if(method.HasGenericParameters)
            {
                signatureBuilder.Append("<");
                signatureBuilder.AppendJoin(", ", method.GenericParameters.Select(x => x.Name));
                signatureBuilder.Append(">");
            }

            signatureBuilder.Append("(");
            signatureBuilder.AppendJoin(
                ", ",
                method.Parameters.Select(p => $"{p.ParameterType.ToTypeId().DisplayName} {p.Name}")
            );
            signatureBuilder.Append(")");

            return signatureBuilder.ToString();
        }
    }
}
