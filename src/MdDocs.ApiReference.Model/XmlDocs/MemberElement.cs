using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
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
        /// Supported for types, fields, events, properties, indexers and methods
        /// </para>       
        /// </remarks>
        /// <value>The specified summary or <c>null</c> if no summary was specified</value>
        public TextBlock Summary { get; set; }

        /// <summary>
        /// Gets the remarks for a member
        /// </summary>
        /// <remarks>
        /// Text specified using the <c>remarks</c> tag provides more detailed information about
        /// a member supplementing information from <c>summary</c>
        /// <para>
        /// Supported for types, fields, events, properties, indexers and methods
        /// </para>
        /// </remarks>
        /// <value>The specified remarks or <c>null</c> if no remarks are available</value>
        public TextBlock Remarks { get; set; }

        public TextBlock Example { get; set; }

        public List<ExceptionElement> Exceptions { get; } = new List<ExceptionElement>();

        /// <summary>
        /// Gets the documentation for the member's type parameters
        /// </summary>
        /// <remarks>
        /// Text specified using the <c>typeparam</c> element.
        /// <para>
        /// Supported for types and methods
        /// </para>
        /// </remarks>
        /// <value>
        /// A dictionary containing all available type parameter documentation. The name of the type parameter serves as key into the dictionary.
        /// Value is never null and is empty if no type parameter documentation was specified.
        /// </value>
        public IDictionary<string, TextBlock> TypeParameters { get; } = new Dictionary<string, TextBlock>();

        /// <summary>
        /// Gets the documentation for the member's parameters
        /// </summary>
        /// <remarks>
        /// Text specified using the <c>param</c> element.
        /// <para>
        /// Supported for indexers and methods
        /// </para>
        /// </remarks>
        /// <value>
        /// A dictionary containing all available parameter documentation. The name of the parameter serves as key into the dictionary.
        /// Value is never null and is empty if no parameter documentation was specified.
        /// </value>
        public IDictionary<string, TextBlock> Parameters { get; } = new Dictionary<string, TextBlock>();

        /// <summary>
        /// Gets the <c>value</c> documentation for the member
        /// </summary>
        /// <remarks>
        /// <c>value</c> describes the value a property, indexer or field represents
        /// <para>
        /// Supported for fields, properties and indexers
        /// </para>
        /// </remarks>
        /// <value>The specified text or <c>null</c> if no text is available</value>
        public TextBlock Value { get; set; }

        public TextBlock Returns { get; set; }

        /// <summary>
        /// Gets the items specified for the member using the <c>seealso</c> tag.
        /// </summary>
        /// <value>
        /// The list of <c>seealso</c> elements specified for the member.
        /// Empty list when no <c>seealso</c> items were specified
        /// </value>
        public List<SeeAlsoElement> SeeAlso { get; } = new List<SeeAlsoElement>();


        public MemberElement(MemberId memberId)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
        }
    }
}
