using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base documentation model for all type members (field, events, methods...)
    /// </summary>
    public abstract class _MemberDocumentation
    // TODO 2021-08-05: Implement IAssemblyMemberDocumentation ?
    {
        /// <summary>
        /// Gets the documentation model for the type defining the member.
        /// </summary>
        public _TypeDocumentation DeclaringType { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public AssemblyDocumentation AssemblyDocumentation => TypeDocumentation.GetAssemblyDocumentation();


        // private protected constructor => prevent implementation outside of this assembly
        private protected _MemberDocumentation(_TypeDocumentation declaringType)
        {
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }
    }
}
