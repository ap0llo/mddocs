using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base documentation model for non-overloadable type members (fields, events, properties)
    /// </summary>
    public abstract class _SimpleMemberDocumentation : _MemberDocumentation
    // TODO 2021-08-05: Implement IObsoleteableDocumentation ?
    {
        /// <summary>
        /// Gets the id of the member.
        /// </summary>
        public abstract MemberId MemberId { get; }

        ///// <summary>
        ///// Gets the <c>summary</c> documentation for this member.
        ///// </summary>
        // TODO 2021-08-05: public TextBlock Summary { get; }

        ///// <summary>
        ///// Gets the <c>remarks</c> documentation for this member.
        ///// </summary>
        // TODO 2021-08-05: public TextBlock Remarks { get; }

        ///// <summary>
        ///// Gets the <c>seealso</c> documentation items for this member.
        ///// </summary>
        // TODO 2021-08-05: public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        /// <summary>
        /// Gets this member's name.
        /// </summary>
        public abstract string Name { get; }

        ///// <summary>
        ///// Gets the definition of the member as C# code.
        ///// </summary>
        // TODO 2021-08-05: public abstract string CSharpDefinition { get; }

        /// <summary>
        /// Gets this member's type.
        /// </summary>
        public abstract TypeId Type { get; }

        ///// <summary>
        ///// Gets the <c>example</c> documentation for this member.
        ///// </summary>
        // TODO 2021-08-05: public TextBlock Example { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public bool IsObsolete { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public string? ObsoleteMessage { get; }


        // private protected constructor => prevent implementation outside of this assembly
        private protected _SimpleMemberDocumentation(_TypeDocumentation declaringType) : base(declaringType)
        { }
    }
}
