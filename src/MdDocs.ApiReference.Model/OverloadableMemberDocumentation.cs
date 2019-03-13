using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base class for type members that can be overloaded (e.g. methods, indexers...)
    /// </summary>
    /// <typeparam name="TOverload">The model type of the overload.</typeparam>
    public abstract class OverloadableMemberDocumentation<TOverload> : MemberDocumentation where TOverload : OverloadDocumentation
    {
        /// <summary>
        /// Gets the member's overloads.
        /// </summary>
        public abstract IReadOnlyCollection<TOverload> Overloads { get; }


        // private protected constructor => prevent implementation outside of this assembly
        private protected OverloadableMemberDocumentation(TypeDocumentation typeDocumentation) : base(typeDocumentation)
        {
        }
    }
}
