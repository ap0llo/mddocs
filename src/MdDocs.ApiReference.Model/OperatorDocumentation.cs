using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for overloaded operators (e.g. +, &, ..)
    /// </summary>
    /// <remarks>
    /// For documentation on operator overloading in C#, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/overloadable-operators
    /// </remarks>
    public sealed class OperatorDocumentation : OverloadableMemberDocumentation<OperatorOverloadDocumentation>
    {
        private readonly IDictionary<MemberId, OperatorOverloadDocumentation> m_Overloads;

        /// <summary>
        /// Gets the operator being overloaded.
        /// </summary>
        public OperatorKind Kind { get; }

        /// <inheritDoc />
        public override IReadOnlyCollection<OperatorOverloadDocumentation> Overloads { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="OperatorDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the operator overload.</param>
        /// <param name="definitions">The underlying Mono.Cecul definitions of the operator overload.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal OperatorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            if (definitions.Select(x => x.Name).Distinct().Count() > 1)
                throw new ArgumentException("All definitions have to be overloads of the same method", nameof(definitions));

            m_Overloads = definitions
                .Select(d => new OperatorOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(x => x.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            var operatorKinds = Overloads.Select(x => x.OperatorKind).Distinct().ToArray();

            Kind = operatorKinds.Length == 1
                ? operatorKinds[0]
                : throw new ArgumentException("Cannot combine overloads of different operators");
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id)
        {
            if (id is MethodId methodId &&
               methodId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               methodId.GetOperatorKind() == Kind)
            {
                return m_Overloads.GetValueOrDefault(methodId);
            }
            else
            {
                return TypeDocumentation.TryGetDocumentation(id);
            }
        }
    }
}
