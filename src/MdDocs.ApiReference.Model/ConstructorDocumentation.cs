using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a class or struct constructor.
    /// </summary>
    public sealed class ConstructorDocumentation : OverloadableMemberDocumentation<ConstructorOverloadDocumentation>
    {
        private readonly string m_Name;
        private readonly IDictionary<MemberId, ConstructorOverloadDocumentation> m_Overloads;

        /// <inheritDoc />
        public override IReadOnlyCollection<ConstructorOverloadDocumentation> Overloads { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ConstructorDocumentation"/>
        /// </summary>
        /// <param name="typeDocumentation">The documentation model for the type that defines the constructor.</param>
        /// <param name="definitions">The definitions of the type's constructor</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor arguments is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="definitions"/> contains definitions with different method names.</exception>
        internal ConstructorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            if (xmlDocsProvider == null)
                throw new ArgumentNullException(nameof(xmlDocsProvider));

            if (definitions.Select(x => x.Name).Distinct().Count() > 1)
                throw new ArgumentException("All definitions have to be overloads of the same method", nameof(definitions));

            m_Overloads = definitions
                .Select(d => new ConstructorOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(d => d.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            m_Name = definitions.First().Name;
        }


        /// <inheritdoc />
        public override IDocumentation TryGetDocumentation(MemberId id)
        {
            if (id is MethodId methodId &&
               methodId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               methodId.Name == m_Name)
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
