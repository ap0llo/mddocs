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

            var operatorKind = method.GetOperatorKind();
            if(method.IsConstructor)
            {
                signatureBuilder.Append(method.DeclaringType.Name);
            }
            else if(operatorKind.HasValue)
            {
                signatureBuilder.Append(operatorKind.Value);
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
            if(operatorKind == OperatorKind.Implicit || operatorKind == OperatorKind.Explicit)
            {
                signatureBuilder.Append(method.Parameters[0].ParameterType.ToTypeId().DisplayName);
                signatureBuilder.Append(" to ");
                signatureBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
            }
            else
            {
                signatureBuilder.AppendJoin(
                    ", ",
                    method.Parameters.Select(p => p.ParameterType.ToTypeId().DisplayName)
                );
            }
            signatureBuilder.Append(")");

            return signatureBuilder.ToString();
        }

    }
}
