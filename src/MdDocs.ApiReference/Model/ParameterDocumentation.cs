using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public sealed class ParameterDocumentation : IDocumentation
    {
        public string Name => Definition.Name;

        public TypeId ParameterType { get; }

        public OverloadDocumentation OverloadDocumentation { get; }

        /// <summary>
        /// Gets the parameters documentation
        /// </summary>
        /// <value>Gets the documentaton for the parameter (specified using the <c>param</c> tag) or <c>null</c> if no documentaiton is available</value>
        public TextBlock Description { get; }

        internal ParameterDefinition Definition { get; }


        internal ParameterDocumentation(OverloadDocumentation overloadDocumentation, ParameterDefinition definition, IXmlDocsProvider xmlDocsProvider)
        {
            OverloadDocumentation = overloadDocumentation ?? throw new ArgumentNullException(nameof(overloadDocumentation));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            ParameterType = definition.ParameterType.ToTypeId();

            Description = xmlDocsProvider.TryGetDocumentationComments(overloadDocumentation.MemberId)?.Parameters?.GetValueOrDefault(definition.Name);
        }


        public IDocumentation TryGetDocumentation(MemberId id) => 
            OverloadDocumentation.TryGetDocumentation(id);
    }
}
