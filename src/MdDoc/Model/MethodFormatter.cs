using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    sealed class MethodFormatter
    {
        public static readonly MethodFormatter Instance = new MethodFormatter();

        private readonly TypeNameFormatter m_TypeNameFormatter = TypeNameFormatter.Instance;


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
                method.Parameters.Select(p => $"{m_TypeNameFormatter.GetTypeName(p.ParameterType)} {p.Name}")
            );
            signatureBuilder.Append(")");

            return signatureBuilder.ToString();
        }

    }
}
