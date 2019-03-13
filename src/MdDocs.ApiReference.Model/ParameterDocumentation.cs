using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a method or indexer parameter.
    /// </summary>
    public sealed class ParameterDocumentation : IDocumentation
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name => Definition.Name;

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public TypeId ParameterType { get; }

        /// <summary>
        /// Gets the overload that defines the parameter.
        /// </summary>
        public OverloadDocumentation OverloadDocumentation { get; }

        /// <summary>
        /// Gets the parameters documentation
        /// </summary>
        /// <value>Gets the documentation for the parameter (specified using the <c>param</c> tag) or <c>null</c> if no documentation is available</value>
        public TextBlock Description { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the parameter.
        /// </summary>
        internal ParameterDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ParameterDocumentation"/>.
        /// </summary>
        /// <param name="overloadDocumentation">The overload that defines the parameter.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the parameter.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal ParameterDocumentation(OverloadDocumentation overloadDocumentation, ParameterDefinition definition, IXmlDocsProvider xmlDocsProvider)
        {
            OverloadDocumentation = overloadDocumentation ?? throw new ArgumentNullException(nameof(overloadDocumentation));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            ParameterType = definition.ParameterType.ToTypeId();

            Description = xmlDocsProvider.TryGetDocumentationComments(overloadDocumentation.MemberId)?.Parameters?.GetValueOrDefault(definition.Name);
        }


        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId id) =>
            OverloadDocumentation.TryGetDocumentation(id);
    }
}
