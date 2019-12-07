using System.Linq;
using System.Text;
using Grynwald.Utilities.Text;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Helper class to the signature of members from Mono.Cecil definitions.
    /// </summary>
    /// <example>
    /// For a method named "MyMethod" with a single string parameter,
    /// <see cref="GetSignature(MethodDefinition)" /> would return <c>MyMethod(string)</c>.
    /// </example>
    public static class SignatureFormatter
    {
        /// <summary>
        /// Gets the signature of the specified method.
        /// </summary>
        public static string GetSignature(MethodDefinition method)
        {
            var signatureBuilder = new StringBuilder();

            var operatorKind = method.GetOperatorKind();

            if (method.IsConstructor)
            {
                var name = method.DeclaringType.Name;

                // remove number of type parameters from name
                if (method.DeclaringType.HasGenericParameters)
                {
                    name = name.Substring(0, name.LastIndexOf('`'));
                }

                signatureBuilder.Append(name);
            }
            else if (operatorKind.HasValue)
            {
                signatureBuilder.Append(operatorKind.Value);
            }
            else
            {
                signatureBuilder.Append(method.Name);
            }

            if (method.HasGenericParameters)
            {
                signatureBuilder.Append("<");
                signatureBuilder.AppendJoin(", ", method.GenericParameters.Select(x => x.Name));
                signatureBuilder.Append(">");
            }


            signatureBuilder.Append("(");
            if (operatorKind == OperatorKind.Implicit || operatorKind == OperatorKind.Explicit)
            {
                signatureBuilder.Append(method.Parameters[0].ParameterType.ToTypeId().DisplayName);
                signatureBuilder.Append(" to ");
                signatureBuilder.Append(method.ReturnType.ToTypeId().DisplayName);
            }
            else
            {
                signatureBuilder.AppendJoin(
                    ", ",
                    method.Parameters.Select(p => GetParameterSignature(p.ParameterType.ToTypeId()))
                );
            }
            signatureBuilder.Append(")");

            return signatureBuilder.ToString();
        }

        /// <summary>
        /// Gets the signature of the specified method.
        /// </summary>
        public static string GetSignature(MethodId method)
        {
            var signatureBuilder = new StringBuilder();

            var operatorKind = method.GetOperatorKind();
            if (method.IsConstructor())
            {
                signatureBuilder.Append(method.DefiningType.Name);
            }
            else if (operatorKind.HasValue)
            {
                signatureBuilder.Append(operatorKind.Value);
            }
            else
            {
                signatureBuilder.Append(method.Name);
            }

            // Method does not have info about generic parameter names, so the parameter will just be called "T"
            if (method.Arity == 1)
            {
                signatureBuilder.Append("<T>");
            }
            // Method does not have info about generic parameter names, so they will just be named T1..Tn
            else if (method.Arity > 1)
            {
                signatureBuilder.Append("<");
                signatureBuilder.AppendJoin(", ", Enumerable.Range(1, method.Arity).Select(i => $"T{i}"));
                signatureBuilder.Append(">");
            }

            signatureBuilder.Append("(");
            if (operatorKind == OperatorKind.Implicit || operatorKind == OperatorKind.Explicit)
            {
                signatureBuilder.Append(method.Parameters[0].DisplayName);
                signatureBuilder.Append(" to ");
                signatureBuilder.Append(method.ReturnType.DisplayName);
            }
            else
            {
                signatureBuilder.AppendJoin(
                    ", ",
                    method.Parameters.Select(GetParameterSignature)
                );
            }
            signatureBuilder.Append(")");

            return signatureBuilder.ToString();
        }

        /// <summary>
        /// Gets the signature of the specified property.
        /// </summary>
        public static string GetSignature(PropertyDefinition property)
        {
            var signatureBuilder = new StringBuilder();

            signatureBuilder.Append(property.Name);


            if (property.HasParameters)
            {
                signatureBuilder.Append("[");

                signatureBuilder.AppendJoin(
                    ", ",
                    property.Parameters.Select(p => p.ParameterType.ToTypeId().DisplayName)
                );
                signatureBuilder.Append("]");
            }

            return signatureBuilder.ToString();
        }

        /// <summary>
        /// Gets the signature of the specified property.
        /// </summary>
        public static string GetSignature(PropertyId property)
        {
            var signatureBuilder = new StringBuilder();

            signatureBuilder.Append(property.Name);

            if (property.Parameters.Count > 0)
            {
                signatureBuilder.Append("[");

                signatureBuilder.AppendJoin(
                    ", ",
                    property.Parameters.Select(p => p.DisplayName)
                );
                signatureBuilder.Append("]");
            }

            return signatureBuilder.ToString();
        }


        private static string GetParameterSignature(TypeId parameterType)
        {
            if (parameterType is ByReferenceTypeId byReferenceType)
            {
                return byReferenceType.ElementType.DisplayName;
            }
            else
            {
                return parameterType.DisplayName;
            }
        }
    }
}
