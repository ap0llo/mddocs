using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for methods.
    /// </summary>
    public sealed class MethodDocumentation : OverloadableMemberDocumentation<MethodOverloadDocumentation>
    {
        private readonly IDictionary<MemberId, MethodOverloadDocumentation> m_Overloads;


        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name { get; }

        /// <inheritDoc />
        public override IReadOnlyCollection<MethodOverloadDocumentation> Overloads { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="MethodDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the method.</param>
        /// <param name="definitions">The Mono.Cecil definitions for all the method's overloads.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            if (definitions.Select(x => x.Name).Distinct().Count() > 1)
                throw new ArgumentException("All definitions have to be overloads of the same method", nameof(definitions));

            m_Overloads = definitions
                .Select(d => new MethodOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(d => d.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            Name = definitions.First().Name;
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id)
        {
            if (id is MethodId methodId &&
               methodId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               methodId.Name == Name)
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
