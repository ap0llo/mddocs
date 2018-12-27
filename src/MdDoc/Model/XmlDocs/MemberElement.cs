using System;
using System.Collections.Generic;

namespace MdDoc.Model.XmlDocs
{
    public sealed class MemberElement
    {
        public MemberId MemberId { get; }

        /// <summary>
        /// Gets the summary for a member
        /// </summary>
        /// <remarks>
        /// Text specified using the <c>summary</c> tag.
        /// <para>
        /// Supported for types, fields, events, properties and methods
        /// </para>       
        /// </remarks>
        /// <value>The specified summary or <c>null</c> if no summary was specified</value>
        public TextBlock Summary { get; }

        /// <summary>
        /// Gets the remarks for a member
        /// </summary>
        /// <remarks>
        /// Text specified using the <c>remarks</c> tag provides more detailed information about
        /// a member supplementing information from <c>summary</c>
        /// <para>
        /// Supported for types, fields, events, properties and methods
        /// </para>
        /// </remarks>
        /// <value>The specified remarks or <c>null</c> if no remarks are available</value>
        public TextBlock Remarks { get; }

        public TextBlock Example { get; }

        public IReadOnlyList<ExceptionElement> Exceptions { get; }

        public IReadOnlyList<TypeParamElement> TypeParameters { get; }

        public IReadOnlyList<ParamElement> Parameters { get; }

        public TextBlock Value { get; }

        public TextBlock Returns { get; }

        /// <summary>
        /// Gets the items specified for the member using the <c>seealso</c> tag.
        /// </summary>
        /// <value>
        /// The list of <c>seealso</c> elements specified for the member.
        /// Empty list when no <c>seealso</c> items were specified
        /// </value>
        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }


        public MemberElement(
            MemberId memberId,
            TextBlock summary,
            TextBlock remarks,
            TextBlock example,
            IReadOnlyList<ExceptionElement> exceptions, 
            IReadOnlyList<TypeParamElement> typeParameters,
            IReadOnlyList<ParamElement> parameters,
            TextBlock value,
            TextBlock returns,
            IReadOnlyList<SeeAlsoElement> seeAlso)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Summary = summary;
            Remarks = remarks;
            Example = example;
            Exceptions = exceptions ?? Array.Empty<ExceptionElement>();
            TypeParameters = typeParameters ?? Array.Empty<TypeParamElement>();
            Parameters = parameters ?? Array.Empty<ParamElement>();
            Value = value;
            Returns = returns;
            SeeAlso = seeAlso ?? Array.Empty<SeeAlsoElement>();
        }
        
    }
}
